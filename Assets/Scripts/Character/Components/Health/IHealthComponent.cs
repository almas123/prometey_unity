public interface IHealthComponent
{
    float Health { get; }
    float MaxHealth { get; }

    void SetDamage(float damage);
}
