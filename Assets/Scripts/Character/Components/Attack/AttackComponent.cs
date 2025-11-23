using UnityEngine;

public class AttackComponent : IAttackComponent
{
    public float Damage => 10;
    public float AttackRange => 2;
    public float AttackZone => 5;

    private Character character;
    private float lastAttackTime;
    private float attackCooldown = 1f;

    public void MakeDamage(Character attackTarget)
    {
        if (Time.time - lastAttackTime < attackCooldown)
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
}