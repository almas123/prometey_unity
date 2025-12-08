using UnityEngine;

/// <summary>
/// Attack parameters configuration.
/// ScriptableObject allows creating different attack profiles (Open/Closed Principle).
/// </summary>
[CreateAssetMenu(fileName = "AttackConfig", menuName = "Configs/Character/Attack Config")]
public class AttackConfig : ScriptableObject
{
    [Header("Damage")]
    [Tooltip("Damage dealt per attack")]
    [SerializeField] private float damage = 10f;

    [Header("Range")]
    [Tooltip("Attack range (distance for dealing damage)")]
    [SerializeField] private float attackRange = 2f;

    [Tooltip("Target detection zone (aggression radius)")]
    [SerializeField] private float attackZone = 30f;

    [Header("Cooldown")]
    [Tooltip("Delay between attacks in seconds")]
    [SerializeField] private float attackCooldown = 1f;

    public float Damage => damage;
    public float AttackRange => attackRange;
    public float AttackZone => attackZone;
    public float AttackCooldown => attackCooldown;

    private void OnValidate()
    {
        // Validate values
        if (damage < 0f)
            damage = 0f;

        if (attackRange < 0f)
            attackRange = 0f;

        if (attackZone < attackRange)
            attackZone = attackRange;

        if (attackCooldown < 0.1f)
            attackCooldown = 0.1f;
    }
}
