using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base generic class for managing object spawning.
/// Implements common spawn logic (DRY principle).
/// Child classes define specific logic via abstract methods (Open/Closed, Template Method Pattern).
/// </summary>
/// <typeparam name="T">Type of spawned object (must be MonoBehaviour)</typeparam>
public abstract class SpawnController<T> : MonoBehaviour, ISpawner where T : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected SpawnConfig config;

    [Header("Spawn Center")]
    [Tooltip("Spawn center. If null, uses this object's Transform.")]
    [SerializeField] protected Transform spawnCenter;

    [Header("Auto Start")]
    [SerializeField] protected bool autoStart = true;

    // List of active spawned objects
    protected List<T> activeObjects = new List<T>();

    // Spawn timer
    protected float spawnTimer;

    // Spawn active flag
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
        // Validation
        if (config == null)
        {
            Debug.LogError($"{GetType().Name}: SpawnConfig not assigned!");
            return;
        }

        // Set spawn center
        if (spawnCenter == null)
        {
            spawnCenter = transform;
        }

        // Auto start if enabled
        if (autoStart)
        {
            StartSpawning();
        }
    }

    protected virtual void Update()
    {
        if (!isSpawning || config == null)
            return;

        // Update timer
        spawnTimer -= Time.deltaTime;

        // Cleanup destroyed objects
        CleanupDestroyedObjects();

        // Check if can spawn
        if (spawnTimer <= 0f && CanSpawn())
        {
            TrySpawnObject();
            spawnTimer = config.SpawnInterval;
        }
    }

    #endregion

    #region Protected Methods (Common Logic)

    /// <summary>
    /// Attempts to spawn an object.
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
    /// Generates random position around spawn center.
    /// </summary>
    /// <returns>Random position within specified radius</returns>
    protected virtual Vector3 GetRandomSpawnPosition()
    {
        if (spawnCenter == null)
        {
            Debug.LogWarning($"{GetType().Name}: Spawn center is null!");
            return Vector3.zero;
        }

        // Generate random angle
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        // Generate random distance within range
        float distance = Random.Range(config.MinSpawnDistance, config.MaxSpawnDistance);

        // Calculate position relative to center
        Vector3 offset = new Vector3(
            Mathf.Cos(angle) * distance,
            0f,
            Mathf.Sin(angle) * distance
        );

        Vector3 spawnPosition = spawnCenter.position + offset;
        spawnPosition.y = spawnCenter.position.y; // Preserve Y coordinate of center

        return spawnPosition;
    }

    /// <summary>
    /// Cleans list from destroyed objects.
    /// </summary>
    protected virtual void CleanupDestroyedObjects()
    {
        activeObjects.RemoveAll(obj => obj == null);
    }

    /// <summary>
    /// Checks if new object can be spawned.
    /// </summary>
    /// <returns>true if can spawn, false otherwise</returns>
    protected virtual bool CanSpawn()
    {
        return activeObjects.Count < GetMaxObjectCount();
    }

    #endregion

    #region Abstract Methods (Must be implemented in child classes)

    /// <summary>
    /// Creates and initializes new object.
    /// Child classes implement specific spawn logic.
    /// </summary>
    /// <returns>Spawned object or null if spawn failed</returns>
    protected abstract T SpawnObject();

    /// <summary>
    /// Called after successful object spawn.
    /// Used for additional initialization.
    /// </summary>
    /// <param name="spawnedObject">Spawned object</param>
    protected abstract void OnObjectSpawned(T spawnedObject);

    /// <summary>
    /// Returns maximum number of objects that can be spawned simultaneously.
    /// </summary>
    /// <returns>Maximum count</returns>
    protected abstract int GetMaxObjectCount();

    #endregion

    #region Public API

    /// <summary>
    /// Returns list of all active objects.
    /// </summary>
    public IReadOnlyList<T> GetActiveObjects()
    {
        return activeObjects.AsReadOnly();
    }

    /// <summary>
    /// Destroys all active objects.
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
