using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый generic класс для управления спауном объектов.
/// Реализует общую логику спауна (DRY принцип).
/// Наследники определяют специфичную логику через абстрактные методы (Open/Closed, Template Method Pattern).
/// </summary>
/// <typeparam name="T">Тип спаунимого объекта (должен быть MonoBehaviour)</typeparam>
public abstract class SpawnController<T> : MonoBehaviour, ISpawner where T : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected SpawnConfig config;

    [Header("Spawn Center")]
    [Tooltip("Центр спауна. Если null, используется Transform этого объекта.")]
    [SerializeField] protected Transform spawnCenter;

    [Header("Auto Start")]
    [SerializeField] protected bool autoStart = true;

    // Список активных заспауненных объектов
    protected List<T> activeObjects = new List<T>();

    // Таймер для спауна
    protected float spawnTimer;

    // Флаг активности спауна
    protected bool isSpawning = false;

    #region ISpawner Implementation

    public int ActiveObjectCount => activeObjects.Count;

    public virtual void StartSpawning()
    {
        isSpawning = true;
        spawnTimer = config.SpawnInterval;
    }

    public virtual void StopSpawning()
    {
        isSpawning = false;
    }

    #endregion

    #region Unity Lifecycle

    protected virtual void Start()
    {
        // Валидация
        if (config == null)
        {
            Debug.LogError($"{GetType().Name}: SpawnConfig не назначен!");
            return;
        }

        // Установить центр спауна
        if (spawnCenter == null)
        {
            spawnCenter = transform;
        }

        // Автостарт если включен
        if (autoStart)
        {
            StartSpawning();
        }
    }

    protected virtual void Update()
    {
        if (!isSpawning || config == null)
            return;

        // Обновляем таймер
        spawnTimer -= Time.deltaTime;

        // Очищаем уничтоженные объекты
        CleanupDestroyedObjects();

        // Проверяем возможность спауна
        if (spawnTimer <= 0f && CanSpawn())
        {
            TrySpawnObject();
            spawnTimer = config.SpawnInterval;
        }
    }

    #endregion

    #region Protected Methods (Общая логика)

    /// <summary>
    /// Попытка заспаунить объект.
    /// </summary>
    protected virtual void TrySpawnObject()
    {
        T spawnedObject = SpawnObject();

        if (spawnedObject != null)
        {
            activeObjects.Add(spawnedObject);
            OnObjectSpawned(spawnedObject);
        }
    }

    /// <summary>
    /// Генерирует случайную позицию вокруг центра спауна.
    /// </summary>
    /// <returns>Случайная позиция в заданном радиусе</returns>
    protected virtual Vector3 GetRandomSpawnPosition()
    {
        if (spawnCenter == null)
        {
            Debug.LogWarning($"{GetType().Name}: Spawn center is null!");
            return Vector3.zero;
        }

        // Генерируем случайный угол
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        // Генерируем случайное расстояние в заданном диапазоне
        float distance = Random.Range(config.MinSpawnDistance, config.MaxSpawnDistance);

        // Вычисляем позицию относительно центра
        Vector3 offset = new Vector3(
            Mathf.Cos(angle) * distance,
            0f,
            Mathf.Sin(angle) * distance
        );

        Vector3 spawnPosition = spawnCenter.position + offset;
        spawnPosition.y = spawnCenter.position.y; // Сохраняем Y координату центра

        return spawnPosition;
    }

    /// <summary>
    /// Очищает список от уничтоженных объектов.
    /// </summary>
    protected virtual void CleanupDestroyedObjects()
    {
        activeObjects.RemoveAll(obj => obj == null);
    }

    /// <summary>
    /// Проверяет возможность спауна нового объекта.
    /// </summary>
    /// <returns>true если можно спаунить, false иначе</returns>
    protected virtual bool CanSpawn()
    {
        return activeObjects.Count < GetMaxObjectCount();
    }

    #endregion

    #region Abstract Methods (Должны быть реализованы в наследниках)

    /// <summary>
    /// Создает и инициализирует новый объект.
    /// Наследники реализуют специфичную логику спауна.
    /// </summary>
    /// <returns>Заспауненный объект или null если спаун не удался</returns>
    protected abstract T SpawnObject();

    /// <summary>
    /// Вызывается после успешного спауна объекта.
    /// Используется для дополнительной инициализации.
    /// </summary>
    /// <param name="spawnedObject">Заспауненный объект</param>
    protected abstract void OnObjectSpawned(T spawnedObject);

    /// <summary>
    /// Возвращает максимальное количество объектов которые могут быть заспаунены одновременно.
    /// </summary>
    /// <returns>Максимальное количество</returns>
    protected abstract int GetMaxObjectCount();

    #endregion

    #region Public API

    /// <summary>
    /// Возвращает список всех активных объектов.
    /// </summary>
    public IReadOnlyList<T> GetActiveObjects()
    {
        return activeObjects.AsReadOnly();
    }

    /// <summary>
    /// Уничтожает все активные объекты.
    /// </summary>
    public virtual void ClearAllObjects()
    {
        foreach (var obj in activeObjects)
        {
            if (obj != null)
            {
                Destroy(obj.gameObject);
            }
        }

        activeObjects.Clear();
    }

    #endregion
}
