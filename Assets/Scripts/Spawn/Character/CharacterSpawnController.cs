using UnityEngine;

/// <summary>
/// Enemy character spawn controller.
/// Inherits from SpawnController (DRY principle) and adds enemy-specific logic.
/// Uses GameObjectFinder to find player (DRY).
/// Uses public API SetTarget() instead of reflection (KISS).
/// </summary>
public class CharacterSpawnController : SpawnController<EnemyCharacter>
{
    private CharacterSpawnConfig characterConfig;
    private Character playerCharacter;
    private float gameTime;
    private int currentMaxEnemies;

    protected override void Start()
    {
        // Validate config
        if (config is CharacterSpawnConfig charConfig)
        {
            characterConfig = charConfig;
        }
        else
        {
            Debug.LogError("CharacterSpawnController: Config must be of type CharacterSpawnConfig!");
            return;
        }

        // Find player via utility (DRY principle)
        playerCharacter = GameObjectFinder.FindPlayer<Character>();
        if (playerCharacter == null)
        {
            Debug.LogError("CharacterSpawnController: Player not found!");
            return;
        }

        // Set spawn center to player if not specified
        if (spawnCenter == null)
        {
            spawnCenter = playerCharacter.transform;
        }

        // Initialize difficulty
        currentMaxEnemies = characterConfig.startingMaxEnemies;

        // Call base Start
        base.Start();
    }

    protected override void Update()
    {
        // Update game time for difficulty system
        gameTime += Time.deltaTime;
        UpdateDifficulty();

        // Call base Update (timer and spawn management)
        base.Update();
    }

    #region Difficulty Scaling

    /// <summary>
    /// Updates game difficulty based on time elapsed.
    /// </summary>
    private void UpdateDifficulty()
    {
        float minutesPassed = gameTime / 60f;
        int additionalEnemies = Mathf.FloorToInt(minutesPassed * characterConfig.maxEnemiesIncreasePerMinute);
        currentMaxEnemies = Mathf.Min(
            characterConfig.startingMaxEnemies + additionalEnemies,
            characterConfig.absoluteMaxEnemies
        );
    }

    #endregion

    #region SpawnController Implementation

    protected override EnemyCharacter SpawnObject()
    {
        if (characterConfig.enemyPrefab == null)
        {
            Debug.LogWarning("CharacterSpawnController: Enemy prefab not assigned!");
            return null;
        }

        // Get random position from base class
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Create enemy
        GameObject enemyObject = Instantiate(characterConfig.enemyPrefab, spawnPosition, Quaternion.identity);
        EnemyCharacter enemy = enemyObject.GetComponent<EnemyCharacter>();

        if (enemy == null)
        {
            Debug.LogWarning("CharacterSpawnController: Prefab doesn't have EnemyCharacter component!");
            Destroy(enemyObject);
            return null;
        }

        return enemy;
    }

    protected override void OnObjectSpawned(EnemyCharacter enemy)
    {
        // Set target to enemy via public API (KISS principle, no reflection)
        enemy.SetTarget(playerCharacter);
    }

    protected override int GetMaxObjectCount()
    {
        return currentMaxEnemies;
    }

    protected override void CleanupDestroyedObjects()
    {
        // Remove null objects
        base.CleanupDestroyedObjects();

        // Remove dead enemies
        for (int i = activeObjects.Count - 1; i >= 0; i--)
        {
            if (activeObjects[i] != null &&
                activeObjects[i].HealthComponent != null &&
                activeObjects[i].HealthComponent.Health <= 0)
            {
                Destroy(activeObjects[i].gameObject);
                activeObjects.RemoveAt(i);
            }
        }
    }

    #endregion

    #region Public API

    /// <summary>
    /// Get current maximum number of enemies.
    /// </summary>
    public int GetCurrentMaxEnemies()
    {
        return currentMaxEnemies;
    }

    /// <summary>
    /// Get game time in seconds.
    /// </summary>
    public float GetGameTime()
    {
        return gameTime;
    }

    /// <summary>
    /// Get game time in minutes.
    /// </summary>
    public float GetGameTimeInMinutes()
    {
        return gameTime / 60f;
    }

    #endregion
}
