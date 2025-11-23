using System;
using UnityEngine;

public class MovementComponent : IMovementComponent
{
    private float speed;
    private Character character;

    public float Speed
    {
        get => speed;
        set
        {
            if (value < 0)
                speed = 0;
            else
                speed = value;
        }
    }

    public Vector3 Position => character.Data.CharacterTransform.position;

    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        float targetAngle = MathF.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Vector3 movement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        character.Data.CharacterController.Move(movement * speed * Time.deltaTime);
    }

    public void Rotation(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        const float turnSmoothTime = 0.1f;
        float turnSmoothVelocity = 0f;
        float targetAngle = MathF.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(
            character.Data.CharacterTransform.eulerAngles.y,
            targetAngle,
            ref turnSmoothVelocity,
            turnSmoothTime);

        character.Data.CharacterTransform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void Initialize(Character character)
    {
        this.character = character;
        speed = character.Data.DefaultSpeed;
    }
}