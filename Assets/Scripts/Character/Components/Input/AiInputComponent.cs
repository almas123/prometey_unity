using UnityEngine;

public class AiInputComponent : IInputComponent
{
    private Character character;
    private Character target;

    public void Initialize(Character character, Character target)
    {
        this.character = character;
        this.target = target;
    }

    public Vector3 GetMovementInput()
    {
        if (target == null || character == null)
            return Vector3.zero;

        Vector3 direction = target.transform.position - character.transform.position;
        direction.y = 0;
        float distance = direction.magnitude;

        return distance > character.AttackComponent.AttackRange ? direction.normalized : Vector3.zero;
    }
}
