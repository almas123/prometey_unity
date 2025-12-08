using UnityEngine;
using UnityEngine.UI;

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

    private void UpdateTimerDisplay()
    {
        if (timerText == null)
            return;

        float gameTime = spawnController.GetGameTime();
        string formattedTime = FormatTime(gameTime);
        timerText.text = timePrefix + formattedTime;
    }

    private void UpdateEnemyCountDisplay()
    {
        if (enemyCountText == null)
            return;

        int currentEnemies = spawnController.ActiveObjectCount;
        int maxEnemies = spawnController.GetCurrentMaxEnemies();
        enemyCountText.text = $"{enemyPrefix}{currentEnemies}/{maxEnemies}";
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SetSpawnController(CharacterSpawnController controller)
    {
        spawnController = controller;
    }
}
