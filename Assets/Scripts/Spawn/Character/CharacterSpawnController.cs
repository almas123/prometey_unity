using UnityEngine;

/// <summary>
/// Контроллер спауна персонажей-врагов.
/// Наследует от SpawnController (DRY принцип) и добавляет специфику врагов.
/// Использует GameObjectFinder для поиска игрока (DRY).
/// Использует публичный API SetTarget() вместо рефлексии (KISS).
/// </summary>
public class CharacterSpawnController : SpawnController<EnemyCharacter>
{
    private CharacterSpawnConfig characterConfig;
    private Character playerCharacter;
    private float gameTime;
    private int currentMaxEnemies;

    protected override void Start()
    {
        // Валидация конфига
        if (config is CharacterSpawnConfig charConfig)
        {
            characterConfig = charConfig;
        }
        else
        {
            Debug.LogError("CharacterSpawnController: Конфиг должен быть типа CharacterSpawnConfig!");
            return;
        }

        // Поиск игрока через утилиту (DRY принцип)
        playerCharacter = GameObjectFinder.FindPlayer<Character>();
        if (playerCharacter == null)
        {
            Debug.LogError("CharacterSpawnController: Игрок не найден!");
            return;
        }

        // Установить центр спауна на игрока если не указан
        if (spawnCenter == null)
        {
            spawnCenter = playerCharacter.transform;
        }

        // Инициализация сложности
        currentMaxEnemies = characterConfig.startingMaxEnemies;

        // Вызов базового Start
        base.Start();
    }

    protected override void Update()
    {
        // Обновляем игровое время для системы сложности
        gameTime += Time.deltaTime;
        UpdateDifficulty();

        // Вызов базового Update (управление таймером и спауном)
        base.Update();
    }

    #region Difficulty Scaling

    /// <summary>
    /// Обновляет сложность игры в зависимости от времени.
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
            Debug.LogWarning("CharacterSpawnController: Enemy prefab не установлен!");
            return null;
        }

        // Получить случайную позицию из базового класса
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Создать врага
        GameObject enemyObject = Instantiate(characterConfig.enemyPrefab, spawnPosition, Quaternion.identity);
        EnemyCharacter enemy = enemyObject.GetComponent<EnemyCharacter>();

        if (enemy == null)
        {
            Debug.LogWarning("CharacterSpawnController: У префаба нет компонента EnemyCharacter!");
            Destroy(enemyObject);
            return null;
        }

        return enemy;
    }

    protected override void OnObjectSpawned(EnemyCharacter enemy)
    {
        // Установить цель врагу через публичный API (KISS принцип, без рефлексии)
        enemy.SetTarget(playerCharacter);
    }

    protected override int GetMaxObjectCount()
    {
        return currentMaxEnemies;
    }

    protected override void CleanupDestroyedObjects()
    {
        // Удаляем null объекты
        base.CleanupDestroyedObjects();

        // Удаляем мертвых врагов
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
    /// Получить текущее максимальное количество врагов.
    /// </summary>
    public int GetCurrentMaxEnemies()
    {
        return currentMaxEnemies;
    }

    /// <summary>
    /// Получить игровое время в секундах.
    /// </summary>
    public float GetGameTime()
    {
        return gameTime;
    }

    /// <summary>
    /// Получить игровое время в минутах.
    /// </summary>
    public float GetGameTimeInMinutes()
    {
        return gameTime / 60f;
    }

    #endregion
}
