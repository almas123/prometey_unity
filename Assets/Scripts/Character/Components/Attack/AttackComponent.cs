using UnityEngine;

public class AttackComponent : IAttackComponent
{
    private AttackConfig config;
    private Character character;
    private float lastAttackTime;

    // Значения по умолчанию для обратной совместимости
    private const float DefaultDamage = 10f;
    private const float DefaultAttackRange = 2f;
    private const float DefaultAttackZone = 30f;
    private const float DefaultAttackCooldown = 1f;

    public float Damage => config != null ? config.Damage : DefaultDamage;
    public float AttackRange => config != null ? config.AttackRange : DefaultAttackRange;
    public float AttackZone => config != null ? config.AttackZone : DefaultAttackZone;

    private float AttackCooldown => config != null ? config.AttackCooldown : DefaultAttackCooldown;

    public void MakeDamage(Character attackTarget)
    {
        if (Time.time - lastAttackTime < AttackCooldown)
            return;

        Vector3 direction = character.Data.CharacterTransform.position - attackTarget.transform.position;
        direction.y = 0;

        if (direction.magnitude <= AttackRange)
        {
            attackTarget.HealthComponent.SetDamage(Damage);
            lastAttackTime = Time.time;
        }
    }

    public void Initialize(Character character)
    {
        this.character = character;
    }

    /// <summary>
    /// Инициализация с конфигурацией.
    /// </summary>
    public void Initialize(Character character, AttackConfig attackConfig)
    {
        this.character = character;
        this.config = attackConfig;

        if (config == null)
        {
            Debug.LogWarning($"AttackComponent: AttackConfig не назначен для {character.name}. Используются значения по умолчанию.");
        }
    }
}