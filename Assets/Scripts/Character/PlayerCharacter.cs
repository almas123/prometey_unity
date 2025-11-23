using UnityEngine;

public class PlayerCharacter : Character
{
    protected override void Init()
    {
        base.Init();
        InputComponent = new PlayerInputComponent();
    }
}