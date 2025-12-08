using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays game time and current difficulty information on HUD.
/// Automatically finds CharacterSpawnController to get game time.
/// </summary>
public class GameTimerUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text timerText;
    [SerializeField] private Text enemyCountText;

    [Header("Display Settings")]
    [SerializeField] private bool showEnemyCount = true;
    [SerializeField] private string timePrefix = "Time: ";
    [SerializeField] private string enemyPrefix = "Enemies: ";

    private CharacterSpawnController spawnController;

    private void Start()
    {
        // Find spawn controller in scene
        spawnController = FindObjectOfType<CharacterSpawnController>();

        if (spawnController == null)
        {
            Debug.LogWarning("GameTimerUI: CharacterSpawnController not found in scene!");
        }

        if (timerText == null)
        {
            Debug.LogError("GameTimerUI: Timer Text not assigned!");
        }

        if (showEnemyCount && enemyCountText == null)
        {
            Debug.LogWarning("GameTimerUI: Enemy Count Text not assigned but showEnemyCount is enabled!");
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
