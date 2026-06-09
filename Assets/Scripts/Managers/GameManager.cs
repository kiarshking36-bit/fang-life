using UnityEngine;

/// <summary>
/// GameManager.cs
/// Main game controller - manages overall game state
/// </summary>

public class GameManager : MonoBehaviour
{
    private bool gameRunning = true;
    
    private PlayerStats playerStats;
    private DayNightCycle dayNightCycle;
    
    private void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        dayNightCycle = FindObjectOfType<DayNightCycle>();
        
        Debug.Log("Game started! Survive as long as you can!");
    }
    
    private void Update()
    {
        if (!playerStats.IsAlive())
        {
            gameRunning = false;
            Debug.Log("Game Over!");
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
    
    private void QuitGame()
    {
        Debug.Log("Quitting game...");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public bool IsGameRunning()
    {
        return gameRunning;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}