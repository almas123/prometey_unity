using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnController<T> : MonoBehaviour, ISpawner where T : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] protected SpawnConfig config;

    [Header("Spawn Center")]
    [Tooltip("Spawn center. If null, uses this object's Transform.")]
    [SerializeField] protected Transform spawnCenter;

    [Header("Auto Start")]
    [SerializeField] protected bool autoStart = true;

    protected List<T> activeObjects = new List<T>();

    protected float spawnTimer;

    protected bool isSpawning = false;

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

    protected virtual void Start()
    {
        if (config == null)
        {
            Debug.LogError($"{GetType().Name}: SpawnConfig not assigned!");
            return;
        }

        if (spawnCenter == null)
        {
            spawnCenter = transform;
        }

        if (autoStart)
        {
            StartSpawning();
        }
    }

    protected virtual void Update()
    {
        if (!isSpawning || config == null)
            return;

        spawnTimer -= Time.deltaTime;

        CleanupDestroyedObjects();

        if (spawnTimer <= 0f && CanSpawn())
        {
            TrySpawnObject();
            spawnTimer = config.SpawnInterval;
        }
    }

    protected virtual void TrySpawnObject()
    {
        T spawnedObject = SpawnObject();

        if (spawnedObject != null)
        {
            activeObjects.Add(spawnedObject);
            OnObjectSpawned(spawnedObject);
        }
    }

    protected virtual Vector3 GetRandomSpawnPosition()
    {
        if (spawnCenter == null)
        {
            Debug.LogWarning($"{GetType().Name}: Spawn center is null!");
            return Vector3.zero;
        }

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

        float distance = Random.Range(config.MinSpawnDistance, config.MaxSpawnDistance);

        Vector3 offset = new Vector3(
            Mathf.Cos(angle) * distance,
            0f,
            Mathf.Sin(angle) * distance
        );

        Vector3 spawnPosition = spawnCenter.position + offset;
        spawnPosition.y = spawnCenter.position.y;

        return spawnPosition;
    }

    protected virtual void CleanupDestroyedObjects()
    {
        activeObjects.RemoveAll(obj => obj == null);
    }

    protected virtual bool CanSpawn()
    {
        return activeObjects.Count < GetMaxObjectCount();
    }

    protected abstract T SpawnObject();

    protected abstract void OnObjectSpawned(T spawnedObject);

    protected abstract int GetMaxObjectCount();

    public IReadOnlyList<T> GetActiveObjects()
    {
        return activeObjects.AsReadOnly();
    }

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
}
