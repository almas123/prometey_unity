using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private Character characterTarget;
    [SerializeField] private AiState aiState;

    protected override void Init()
    {
        base.Init();
        InputComponent = new AiInputComponent();
        ((AiInputComponent)InputComponent).Initialize(this, characterTarget);
    }

    protected override void Update()
    {
        if (HealthComponent.Health <= 0 || characterTarget == null)
        {
            aiState = AiState.Idle;
            return;
        }

        UpdateAiState();

        switch (aiState)
        {
            case AiState.Idle:
                return;
            case AiState.MoveToTarget:
                base.Update();
                return;
            case AiState.Attack:
                Attack();
                return;
        }
    }

    private void UpdateAiState()
    {
        Vector3 directionToTarget = characterTarget.transform.position - transform.position;
        directionToTarget.y = 0;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget <= AttackComponent.AttackRange)
            aiState = AiState.Attack;
        else if (distanceToTarget <= AttackComponent.AttackZone)
            aiState = AiState.MoveToTarget;
        else
            aiState = AiState.Idle;
    }

    private void Attack()
    {
        Vector3 direction = characterTarget.transform.position - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
            MovementComponent.Rotation(direction.normalized);

        AttackComponent.MakeDamage(characterTarget);
    }

    public void SetTarget(Character target)
    {
        characterTarget = target;

        if (InputComponent is AiInputComponent aiInput)
        {
            aiInput.Initialize(this, target);
        }
    }

    public Character GetTarget()
    {
        return characterTarget;
    }
}