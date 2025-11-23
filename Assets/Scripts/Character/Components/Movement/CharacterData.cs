using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] private float defaultSpeed;

    [SerializeField] private Transform characterTransform;

    [SerializeField] private CharacterController characterController;

    public float DefaultSpeed => defaultSpeed;

    public Transform CharacterTransform => characterTransform;

    public CharacterController CharacterController => characterController;
}