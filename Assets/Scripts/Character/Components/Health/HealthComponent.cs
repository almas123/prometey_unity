using System;
using UnityEngine;

public class HealthComponent : IHealthComponent
{
    private float health;
    private float maxHealth;

    // Default values for backward compatibility
    private const float DefaultMaxHealth = 100f;
    private const float DefaultStartHealth = 100f;

    public float Health
    {
        get => health;
        set
        {
            health = Math.Clamp(value, 0, MaxHealth);
        }
    }

    public float MaxHealth => maxHealth;

    /// <summary>
    /// Default constructor for backward compatibility.
    /// </summary>
    public HealthComponent()
    {
        maxHealth = DefaultMaxHealth;
        health = DefaultStartHealth;
    }

    /// <summary>
    /// Constructor with configuration.
    /// </summary>
    public HealthComponent(HealthConfig config)
    {
        if (config != null)
        {
            maxHealth = config.MaxHealth;
            health = config.StartHealth;
        }
        else
        {
            Debug.LogWarning("HealthComponent: HealthConfig not assigned. Using default values.");
            maxHealth = DefaultMaxHealth;
            health = DefaultStartHealth;
        }
    }

    public void SetDamage(float damage)
    {
        Health -= damage;
    }
}