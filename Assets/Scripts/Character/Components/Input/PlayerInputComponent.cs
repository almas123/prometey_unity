using UnityEngine;

public class PlayerInputComponent : IInputComponent
{
    public Vector3 GetMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        return new Vector3(horizontalInput, 0, verticalInput).normalized;
    }
}
