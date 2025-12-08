using UnityEngine;

/// <summary>
/// Centralized utility for finding game objects.
/// Eliminates code duplication for player finding (DRY principle).
/// </summary>
public static class GameObjectFinder
{
    private const string PlayerTag = "Player";

    /// <summary>
    /// Finds player and returns component of specified type.
    /// </summary>
    /// <typeparam name="T">Component type to find</typeparam>
    /// <returns>Player component or null if not found</returns>
    public static T FindPlayer<T>() where T : Component
    {
        GameObject player = GameObject.FindGameObjectWithTag(PlayerTag);
        if (player == null)
        {
            Debug.LogWarning($"GameObjectFinder: Player with tag '{PlayerTag}' not found!");
            return null;
        }

        T component = player.GetComponent<T>();
        if (component == null)
        {
            Debug.LogWarning($"GameObjectFinder: Player doesn't have component {typeof(T).Name}!");
            return null;
        }

        return component;
    }

    /// <summary>
    /// Finds player Transform.
    /// </summary>
    /// <returns>Player Transform or null if not found</returns>
    public static Transform FindPlayerTransform()
    {
        GameObject player = GameObject.FindGameObjectWithTag(PlayerTag);
        if (player == null)
        {
            Debug.LogWarning($"GameObjectFinder: Player with tag '{PlayerTag}' not found!");
            return null;
        }

        return player.transform;
    }

    /// <summary>
    /// Finds player GameObject.
    /// </summary>
    /// <returns>Player GameObject or null if not found</returns>
    public static GameObject FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag(PlayerTag);
        if (player == null)
        {
            Debug.LogWarning($"GameObjectFinder: Player with tag '{PlayerTag}' not found!");
            return null;
        }

        return player;
    }
}
