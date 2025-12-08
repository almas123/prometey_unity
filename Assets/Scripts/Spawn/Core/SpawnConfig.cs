using UnityEngine;

[CreateAssetMenu(fileName = "SpawnConfig", menuName = "Configs/Spawn/Spawn Config", order = 0)]
public class SpawnConfig : ScriptableObject
{
    [Header("Spawn Timing")]
    [Tooltip("Interval between spawns in seconds")]
    [SerializeField] private float spawnInterval = 2f;

    [Header("Spawn Position")]
    [Tooltip("Minimum distance from spawn center")]
    [SerializeField] private float minSpawnDistance = 10f;

    [Tooltip("Maximum distance from spawn center")]
    [SerializeField] private float maxSpawnDistance = 15f;

    public float SpawnInterval => spawnInterval;
    public float MinSpawnDistance => minSpawnDistance;
    public float MaxSpawnDistance => maxSpawnDistance;

    private void OnValidate()
    {
        if (spawnInterval < 0.1f)
            spawnInterval = 0.1f;

        if (minSpawnDistance < 0f)
            minSpawnDistance = 0f;

        if (maxSpawnDistance < minSpawnDistance)
            maxSpawnDistance = minSpawnDistance;
    }
}
