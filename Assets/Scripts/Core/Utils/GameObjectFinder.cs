using UnityEngine;

public static class GameObjectFinder
{
    private const string PlayerTag = "Player";

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
