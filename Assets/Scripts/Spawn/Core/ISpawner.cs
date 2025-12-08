/// <summary>
/// Interface for all spawners in the game.
/// Follows Interface Segregation Principle (SOLID) - minimal contract.
/// </summary>
public interface ISpawner
{
    /// <summary>
    /// Current number of active objects.
    /// </summary>
    int ActiveObjectCount { get; }

    /// <summary>
    /// Starts object spawning process.
    /// </summary>
    void StartSpawning();

    /// <summary>
    /// Stops object spawning process.
    /// </summary>
    void StopSpawning();
}
