using UnityEngine;

/// <summary>
/// Централизованная утилита для поиска игровых объектов.
/// Устраняет дублирование кода поиска игрока (DRY принцип).
/// </summary>
public static class GameObjectFinder
{
    private const string PlayerTag = "Player";

    /// <summary>
    /// Находит игрока и возвращает компонент указанного типа.
    /// </summary>
    /// <typeparam name="T">Тип компонента для поиска</typeparam>
    /// <returns>Компонент игрока или null если не найден</returns>
    public static T FindPlayer<T>() where T : Component
    {
        GameObject player = GameObject.FindGameObjectWithTag(PlayerTag);
        if (player == null)
        {
            Debug.LogWarning($"GameObjectFinder: Игрок с тегом '{PlayerTag}' не найден!");
            return null;
        }

        T component = player.GetComponent<T>();
        if (component == null)
        {
            Debug.LogWarning($"GameObjectFinder: У игрока нет компонента {typeof(T).Name}!");
            return null;
        }

        return component;
    }

    /// <summary>
    /// Находит Transform игрока.
    /// </summary>
    /// <returns>Transform игрока или null если не найден</returns>
    public static Transform FindPlayerTransform()
    {
        GameObject player = GameObject.FindGameObjectWithTag(PlayerTag);
        if (player == null)
        {
            Debug.LogWarning($"GameObjectFinder: Игрок с тегом '{PlayerTag}' не найден!");
            return null;
        }

        return player.transform;
    }

    /// <summary>
    /// Находит GameObject игрока.
    /// </summary>
    /// <returns>GameObject игрока или null если не найден</returns>
    public static GameObject FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag(PlayerTag);
        if (player == null)
        {
            Debug.LogWarning($"GameObjectFinder: Игрок с тегом '{PlayerTag}' не найден!");
            return null;
        }

        return player;
    }
}
