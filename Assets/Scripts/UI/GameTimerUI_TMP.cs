using UnityEngine;
using TMPro;

/// <summary>
/// Displays game time and current difficulty information on HUD using TextMeshPro.
/// Automatically finds CharacterSpawnController to get game time.
/// Better version with TextMeshPro support for improved text rendering.
/// </summary>
public class GameTimerUI_TMP : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI enemyCountText;

    [Header("Display Settings")]
    [SerializeField] private bool showEnemyCount = true;
    [SerializeField] private string timePrefix = "Time: ";
    [SerializeField] private string enemyPrefix = "Enemies: ";

    [Header("Color Settings")]
    [SerializeField] private bool useColorCoding = false;
    [SerializeField] private Color easyColor = Color.green;
    [SerializeField] private Color mediumColor = Color.yellow;
    [SerializeField] private Color hardColor = Color.red;

    private CharacterSpawnController spawnController;

    private void Start()
    {
        // Find spawn controller in scene
        spawnController = FindObjectOfType<CharacterSpawnController>();

        if (spawnController == null)
        {
            Debug.LogWarning("GameTimerUI_TMP: CharacterSpawnController not found in scene!");
        }

        if (timerText == null)
        {
            Debug.LogError("GameTimerUI_TMP: Timer Text not assigned!");
        }

        if (showEnemyCount && enemyCountText == null)
        {
            Debug.LogWarning("GameTimerUI_TMP: Enemy Count Text not assigned but showEnemyCount is enabled!");
        }
    }

    private void Update()
    {
        if (spawnController == null)
            return;

        UpdateTimerDisplay();

        if (showEnemyCount)
        {
            UpdateEnemyCountDisplay();
        }
    }

    /// <summary>
    /// Updates timer text with formatted game time.
    /// </summary>
    private void UpdateTimerDisplay()
    {
        if (timerText == null)
            return;

        float gameTime = spawnController.GetGameTime();
        string formattedTime = FormatTime(gameTime);
        timerText.text = timePrefix + formattedTime;

        // Optional color coding based on difficulty
        if (useColorCoding)
        {
            float minutes = spawnController.GetGameTimeInMinutes();
            if (minutes < 5f)
                timerText.color = easyColor;
            else if (minutes < 10f)
                timerText.color = mediumColor;
            else
                timerText.color = hardColor;
        }
    }

    /// <summary>
    /// Updates enemy count text with current/max enemies.
    /// </summary>
    private void UpdateEnemyCountDisplay()
    {
        if (enemyCountText == null)
            return;

        int currentEnemies = spawnController.ActiveObjectCount;
        int maxEnemies = spawnController.GetCurrentMaxEnemies();
        enemyCountText.text = $"{enemyPrefix}{currentEnemies}/{maxEnemies}";

        // Optional color coding based on enemy saturation
        if (useColorCoding)
        {
            float saturation = (float)currentEnemies / maxEnemies;
            if (saturation < 0.5f)
                enemyCountText.color = easyColor;
            else if (saturation < 0.8f)
                enemyCountText.color = mediumColor;
            else
                enemyCountText.color = hardColor;
        }
    }

    /// <summary>
    /// Formats time in seconds to MM:SS format.
    /// </summary>
    /// <param name="timeInSeconds">Time to format</param>
    /// <returns>Formatted time string (MM:SS)</returns>
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// Manually set spawn controller reference.
    /// </summary>
    public void SetSpawnController(CharacterSpawnController controller)
    {
        spawnController = controller;
    }
}
