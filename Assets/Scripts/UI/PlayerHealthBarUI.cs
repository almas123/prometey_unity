using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform;
    [SerializeField] private Image fillImage;
    [SerializeField] private Character playerCharacter;

    private float maxWidth;

    private void Start()
    {
        // Если игрок не назначен, ищем через утилиту (DRY принцип)
        if (playerCharacter == null)
        {
            playerCharacter = GameObjectFinder.FindPlayer<Character>();
        }

        if (fillTransform == null)
        {
            Debug.LogError("PlayerHealthBarUI: Fill Transform не назначен!");
        }
        else
        {
            maxWidth = fillTransform.sizeDelta.x;
        }

        if (playerCharacter == null)
        {
            Debug.LogError("PlayerHealthBarUI: Player Character не найден!");
        }
        else if (playerCharacter.HealthComponent == null)
        {
            Debug.LogError("PlayerHealthBarUI: У Player нет HealthComponent!");
        }
    }

    private void Update()
    {
        if (playerCharacter == null || playerCharacter.HealthComponent == null)
            return;

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (fillTransform == null)
            return;

        float healthPercentage = playerCharacter.HealthComponent.Health / playerCharacter.HealthComponent.MaxHealth;

        Vector2 sizeDelta = fillTransform.sizeDelta;
        sizeDelta.x = maxWidth * healthPercentage;
        fillTransform.sizeDelta = sizeDelta;

        if (fillImage != null)
            fillImage.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }

    public void SetPlayer(Character player)
    {
        playerCharacter = player;
    }
}
