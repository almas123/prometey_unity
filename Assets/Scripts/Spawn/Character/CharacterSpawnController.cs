using UnityEngine;

public class CharacterSpawnController : SpawnController<EnemyCharacter>
{
    private CharacterSpawnConfig characterConfig;
    private Character playerCharacter;
    private float gameTime;
    private int currentMaxEnemies;

    protected override void Start()
    {
        if (config is CharacterSpawnConfig charConfig)
        {
            characterConfig = charConfig;
        }
        else
        {
            Debug.LogError("CharacterSpawnController: Config must be of type CharacterSpawnConfig!");
            return;
        }

        playerCharacter = GameObjectFinder.FindPlayer<Character>();
        if (playerCharacter == null)
        {
            Debug.LogError("CharacterSpawnController: Player not found!");
            return;
        }

        if (spawnCenter == null)
        {
            spawnCenter = playerCharacter.transform;
        }

        currentMaxEnemies = characterConfig.startingMaxEnemies;

        base.Start();
    }

    protected override void Update()
    {
        gameTime += Time.deltaTime;
        UpdateDifficulty();

        base.Update();
    }

    private void UpdateDifficulty()
    {
        float minutesPassed = gameTime / 60f;
        int additionalEnemies = Mathf.FloorToInt(minutesPassed * characterConfig.maxEnemiesIncreasePerMinute);
        currentMaxEnemies = Mathf.Min(
            characterConfig.startingMaxEnemies + additionalEnemies,
            characterConfig.absoluteMaxEnemies
        );
    }

    protected override EnemyCharacter SpawnObject()
    {
        if (characterConfig.enemyPrefab == null)
        {
            Debug.LogWarning("CharacterSpawnController: Enemy prefab not assigned!");
            return null;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();

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
        enemy.SetTarget(playerCharacter);
    }

    protected override int GetMaxObjectCount()
    {
        return currentMaxEnemies;
    }

    protected override void CleanupDestroyedObjects()
    {
        base.CleanupDestroyedObjects();

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

    public int GetCurrentMaxEnemies()
    {
        return currentMaxEnemies;
    }

    public float GetGameTime()
    {
        return gameTime;
    }

    public float GetGameTimeInMinutes()
    {
        return gameTime / 60f;
    }
}
