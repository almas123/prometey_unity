using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnController : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    
    [Header("Spawn Parameters")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minSpawnDistance = 10f;
    [SerializeField] private float maxSpawnDistance = 15f;
    
    [Header("Difficulty Scaling")]
    [SerializeField] private int startingMaxEnemies = 10;
    [SerializeField] private int maxEnemiesIncreasePerMinute = 5;
    [SerializeField] private int absoluteMaxEnemies = 100;
    
    private List<GameObject> activeEnemies = new List<GameObject>();
    private float spawnTimer;
    private float gameTime;
    private int currentMaxEnemies;
    
    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
        
        currentMaxEnemies = startingMaxEnemies;
        spawnTimer = spawnInterval;
    }
    
    private void Update()
    {
        if (player == null)
            return;
        
        // Обновляем игровое время
        gameTime += Time.deltaTime;
        
        // Вычисляем максимальное количество врагов на основе времени
        UpdateMaxEnemies();
        
        // Обновляем таймер спавна
        spawnTimer -= Time.deltaTime;
        
        // Удаляем мертвых врагов из списка
        CleanupDeadEnemies();
        
        // Спавним врагов, если нужно
        if (spawnTimer <= 0f && activeEnemies.Count < currentMaxEnemies)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }
    
    private void UpdateMaxEnemies()
    {
        // Вычисляем количество прошедших минут
        float minutesPassed = gameTime / 60f;
        
        // Увеличиваем максимальное количество врагов каждую минуту
        int additionalEnemies = Mathf.FloorToInt(minutesPassed * maxEnemiesIncreasePerMinute);
        currentMaxEnemies = Mathf.Min(startingMaxEnemies + additionalEnemies, absoluteMaxEnemies);
    }
    
    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy prefab не установлен!");
            return;
        }
        
        // Вычисляем случайную позицию вокруг игрока
        Vector3 spawnPosition = GetRandomSpawnPosition();
        
        // Создаем врага
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        
        // Устанавливаем цель врагу (игрока)
        EnemyCharacter enemyCharacter = enemy.GetComponent<EnemyCharacter>();
        if (enemyCharacter != null)
        {
            // Используем рефлексию для установки приватного поля characterTarget
            var field = typeof(EnemyCharacter).GetField("characterTarget", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                Character playerCharacter = player.GetComponent<Character>();
                field.SetValue(enemyCharacter, playerCharacter);
            }
        }
        
        // Добавляем в список активных врагов
        activeEnemies.Add(enemy);
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        // Генерируем случайный угол
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        
        // Генерируем случайное расстояние в заданном диапазоне
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        
        // Вычисляем позицию относительно игрока
        Vector3 offset = new Vector3(
            Mathf.Cos(angle) * distance,
            0f,
            Mathf.Sin(angle) * distance
        );
        
        Vector3 spawnPosition = player.position + offset;
        spawnPosition.y = player.position.y; // Сохраняем Y координату игрока
        
        return spawnPosition;
    }
    
    private void CleanupDeadEnemies()
    {
        // Удаляем уничтоженные объекты из списка
        activeEnemies.RemoveAll(enemy => enemy == null);
        
        // Также удаляем врагов с нулевым здоровьем
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] != null)
            {
                EnemyCharacter enemyChar = activeEnemies[i].GetComponent<EnemyCharacter>();
                if (enemyChar != null && enemyChar.HealthComponent != null && enemyChar.HealthComponent.Health <= 0)
                {
                    Destroy(activeEnemies[i]);
                    activeEnemies.RemoveAt(i);
                }
            }
        }
    }
    
    // Публичные методы для получения информации о состоянии спавнера
    public int GetActiveEnemyCount()
    {
        return activeEnemies.Count;
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
