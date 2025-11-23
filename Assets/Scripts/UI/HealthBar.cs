using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform;
    [SerializeField] private Image fillImage;
    [SerializeField] private Canvas canvas;

    private Character character;
    private Camera mainCamera;
    private float maxWidth;

    private void Start()
    {
        character = GetComponentInParent<Character>();
        mainCamera = Camera.main;

        if (canvas != null)
            canvas.worldCamera = mainCamera;

        if (fillTransform != null)
            maxWidth = fillTransform.sizeDelta.x;
    }

    private void Update()
    {
        if (character == null || character.HealthComponent == null)
            return;

        UpdateHealthBar();
        UpdateRotation();
    }

    private void UpdateHealthBar()
    {
        if (fillTransform == null)
            return;

        float healthPercentage = character.HealthComponent.Health / character.HealthComponent.MaxHealth;

        Vector2 sizeDelta = fillTransform.sizeDelta;
        sizeDelta.x = maxWidth * healthPercentage;
        fillTransform.sizeDelta = sizeDelta;

        if (fillImage != null)
            fillImage.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }

    private void UpdateRotation()
    {
        if (mainCamera != null && canvas != null)
        {
            canvas.transform.LookAt(canvas.transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up);
        }
    }
}
