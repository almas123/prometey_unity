using UnityEngine;

/// <summary>
/// Конфигурация параметров атаки.
/// ScriptableObject позволяет создавать разные профили атаки (Open/Closed Principle).
/// </summary>
[CreateAssetMenu(fileName = "AttackConfig", menuName = "Configs/Character/Attack Config")]
public class AttackConfig : ScriptableObject
{
    [Header("Damage")]
    [Tooltip("Урон наносимый при атаке")]
    [SerializeField] private float damage = 10f;

    [Header("Range")]
    [Tooltip("Дальность атаки (расстояние для нанесения урона)")]
    [SerializeField] private float attackRange = 2f;

    [Tooltip("Зона обнаружения цели (радиус агрессии)")]
    [SerializeField] private float attackZone = 30f;

    [Header("Cooldown")]
    [Tooltip("Задержка между атаками в секундах")]
    [SerializeField] private float attackCooldown = 1f;

    public float Damage => damage;
    public float AttackRange => attackRange;
    public float AttackZone => attackZone;
    public float AttackCooldown => attackCooldown;

    private void OnValidate()
    {
        // Валидация значений
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
