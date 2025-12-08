using UnityEngine;

[CreateAssetMenu(fileName = "HealthConfig", menuName = "Configs/Character/Health Config")]
public class HealthConfig : ScriptableObject
{
    [Header("Health")]
    [Tooltip("Maximum health amount")]
    [SerializeField] private float maxHealth = 100f;

    [Tooltip("Starting health amount (if less than maxHealth, character starts wounded)")]
    [SerializeField] private float startHealth = 100f;

    public float MaxHealth => maxHealth;
    public float StartHealth => startHealth;

    private void OnValidate()
    {
        if (maxHealth < 1f)
            maxHealth = 1f;

        if (startHealth < 0f)
            startHealth = 0f;

        if (startHealth > maxHealth)
            startHealth = maxHealth;
    }
}
