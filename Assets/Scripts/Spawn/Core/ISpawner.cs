public interface ISpawner
{
    int ActiveObjectCount { get; }

    void StartSpawning();

    void StopSpawning();
}
