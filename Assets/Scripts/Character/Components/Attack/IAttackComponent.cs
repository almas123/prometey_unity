public interface IAttackComponent
{
    float Damage { get; }
    float AttackRange { get; }
    float AttackZone { get; }

    void MakeDamage(Character attackTarget);
    void Initialize(Character character);
}