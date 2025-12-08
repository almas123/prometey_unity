using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSpawnConfig", menuName = "Configs/Spawn/Character Spawn Config", order = 1)]
public class CharacterSpawnConfig : SpawnConfig
{
    [Header("Character Settings")]
    [Tooltip("Enemy prefab to spawn")]
    public GameObject enemyPrefab;

    [Header("Difficulty Scaling")]
    [Tooltip("Starting maximum number of enemies")]
    public int startingMaxEnemies = 10;

    [Tooltip("Maximum enemy increase per minute")]
    public int maxEnemiesIncreasePerMinute = 5;

    [Tooltip("Absolute maximum enemies")]
    public int absoluteMaxEnemies = 100;

    private void OnValidate()
    {
        if (startingMaxEnemies < 1)
            startingMaxEnemies = 1;

        if (maxEnemiesIncreasePerMinute < 0)
            maxEnemiesIncreasePerMinute = 0;

        if (absoluteMaxEnemies < startingMaxEnemies)
            absoluteMaxEnemies = startingMaxEnemies;
    }
}
