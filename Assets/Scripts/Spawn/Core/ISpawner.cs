/// <summary>
/// Интерфейс для всех спаунеров в игре.
/// Следует принципу Interface Segregation (SOLID) - минимальный контракт.
/// </summary>
public interface ISpawner
{
    /// <summary>
    /// Текущее количество активных объектов.
    /// </summary>
    int ActiveObjectCount { get; }

    /// <summary>
    /// Начинает процесс спауна объектов.
    /// </summary>
    void StartSpawning();

    /// <summary>
    /// Останавливает процесс спауна объектов.
    /// </summary>
    void StopSpawning();
}
