using UnityEngine;

/// <summary>
/// Конфигурация спауна персонажей.
/// Наследует базовую конфигурацию и добавляет специфичные для персонажей параметры.
/// </summary>
[CreateAssetMenu(fileName = "CharacterSpawnConfig", menuName = "Configs/Spawn/Character Spawn Config", order = 1)]
public class CharacterSpawnConfig : SpawnConfig
{
    [Header("Character Settings")]
    [Tooltip("Префаб врага для спауна")]
    public GameObject enemyPrefab;

    [Header("Difficulty Scaling")]
    [Tooltip("Начальное максимальное количество врагов")]
    public int startingMaxEnemies = 10;

    [Tooltip("Увеличение максимального количества врагов за минуту")]
    public int maxEnemiesIncreasePerMinute = 5;

    [Tooltip("Абсолютный максимум врагов")]
    public int absoluteMaxEnemies = 100;

    private void OnValidate()
    {
        // Валидация значений
        if (startingMaxEnemies < 1)
            startingMaxEnemies = 1;

        if (maxEnemiesIncreasePerMinute < 0)
            maxEnemiesIncreasePerMinute = 0;

        if (absoluteMaxEnemies < startingMaxEnemies)
            absoluteMaxEnemies = startingMaxEnemies;
    }
}
