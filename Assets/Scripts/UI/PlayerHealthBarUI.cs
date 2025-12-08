using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Character playerCharacter;

    private void Start()
    {
        if (playerCharacter == null)
        {
            playerCharacter = FindObjectOfType<PlayerCharacter>();
            if (playerCharacter != null)
            {
                Debug.Log($"PlayerHealthBarUI: Player найден автоматически - {playerCharacter.name}");
            }
        }

        if (fillImage == null)
        {
            Debug.LogError("PlayerHealthBarUI: Fill Image не назначен!");
        }
        else
        {
            Debug.Log("PlayerHealthBarUI: Fill Image назначен!");
        }

        if (playerCharacter == null)
        {
            Debug.LogError("PlayerHealthBarUI: Player Character не найден!");
        }
        else if (playerCharacter.HealthComponent == null)
        {
            Debug.LogError("PlayerHealthBarUI: У Player нет HealthComponent!");
        }
        else
        {
            Debug.Log($"PlayerHealthBarUI: Всё OK! HP: {playerCharacter.HealthComponent.Health}/{playerCharacter.HealthComponent.MaxHealth}");
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
        if (fillImage == null)
            return;

        float healthPercentage = playerCharacter.HealthComponent.Health / playerCharacter.HealthComponent.MaxHealth;

        fillImage.fillAmount = healthPercentage;
        fillImage.color = Color.Lerp(Color.red, Color.green, healthPercentage);

        Debug.Log($"Health: {playerCharacter.HealthComponent.Health}, Percentage: {healthPercentage}, FillAmount: {fillImage.fillAmount}");
    }

    public void SetPlayer(Character player)
    {
        playerCharacter = player;
    }
}
