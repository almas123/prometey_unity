using UnityEngine;

/// <summary>
/// Конфигурация параметров здоровья.
/// ScriptableObject позволяет создавать разные профили здоровья (Open/Closed Principle).
/// </summary>
[CreateAssetMenu(fileName = "HealthConfig", menuName = "Configs/Character/Health Config")]
public class HealthConfig : ScriptableObject
{
    [Header("Health")]
    [Tooltip("Максимальное количество здоровья")]
    [SerializeField] private float maxHealth = 100f;

    [Tooltip("Начальное количество здоровья (если меньше maxHealth, персонаж начнет раненым)")]
    [SerializeField] private float startHealth = 100f;

    public float MaxHealth => maxHealth;
    public float StartHealth => startHealth;

    private void OnValidate()
    {
        // Валидация значений
        if (maxHealth < 1f)
            maxHealth = 1f;

        if (startHealth < 0f)
            startHealth = 0f;

        if (startHealth > maxHealth)
            startHealth = maxHealth;
    }
}
