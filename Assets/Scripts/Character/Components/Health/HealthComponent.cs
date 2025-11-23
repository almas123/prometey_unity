using System;

public class HealthComponent : IHealthComponent
{
    private float health = 100;
    private float maxHealth = 100;

    public float Health
    {
        get => health;
        set
        {
            health = Math.Clamp(value, 0, MaxHealth);
        }
    }

    public float MaxHealth => maxHealth;

    public void SetDamage(float damage)
    {
        Health -= damage;
    }
}