using UnityEngine;

/// <summary>
/// Базовая конфигурация для системы спауна.
/// ScriptableObject позволяет настраивать спаунер через Unity Editor (Open/Closed Principle).
/// </summary>
[CreateAssetMenu(fileName = "SpawnConfig", menuName = "Configs/Spawn/Spawn Config", order = 0)]
public class SpawnConfig : ScriptableObject
{
    [Header("Spawn Timing")]
    [Tooltip("Интервал между спаунами в секундах")]
    [SerializeField] private float spawnInterval = 2f;

    [Header("Spawn Position")]
    [Tooltip("Минимальное расстояние от центра спауна")]
    [SerializeField] private float minSpawnDistance = 10f;

    [Tooltip("Максимальное расстояние от центра спауна")]
    [SerializeField] private float maxSpawnDistance = 15f;

    public float SpawnInterval => spawnInterval;
    public float MinSpawnDistance => minSpawnDistance;
    public float MaxSpawnDistance => maxSpawnDistance;

    private void OnValidate()
    {
        // Валидация значений
        if (spawnInterval < 0.1f)
            spawnInterval = 0.1f;

        if (minSpawnDistance < 0f)
            minSpawnDistance = 0f;

        if (maxSpawnDistance < minSpawnDistance)
            maxSpawnDistance = minSpawnDistance;
    }
}
