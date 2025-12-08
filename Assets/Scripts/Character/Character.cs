using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;

    public CharacterData Data => characterData;
    public IHealthComponent HealthComponent { get; protected set; }
    public IMovementComponent MovementComponent { get; protected set; }
    public IAttackComponent AttackComponent { get; protected set; }
    public IInputComponent InputComponent { get; protected set; }

    protected virtual void Init()
    {
        HealthComponent = new HealthComponent();
        MovementComponent = new MovementComponent();
        MovementComponent.Initialize(this);
        AttackComponent = new AttackComponent();
        AttackComponent.Initialize(this);
    }

    private void Awake()
    {
        Init();
    }

    protected virtual void Update()
    {
        if (HealthComponent.Health <= 0)
            return;

        Vector3 movement = InputComponent.GetMovementInput();

        if (movement != Vector3.zero)
        {
            MovementComponent.Move(movement);
            MovementComponent.Rotation(movement);
        }
    }
}