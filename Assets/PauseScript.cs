using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI; // Drag & drop Pause Menu Canvas here
    public GameObject backgroundBlur; // Drag & drop semi-transparent background image here

    private bool isPaused = false;


    void Update()
    {
        // Toggle Pause Menu with ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC is pressed");
            if (isPaused)

            {
                ResumeGame();
                Debug.Log("Game Resumed!");
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        backgroundBlur.SetActive(true);
        Debug.Log("Game Paused!");
        Time.timeScale = 0f; // Freeze game
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        backgroundBlur.SetActive(false);

        Time.timeScale = 1f; // Resume game

        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time scale before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    public void QuitGame()
    {
        Time.timeScale = 1f; // Reset time scale before quitting
        Application.Quit(); // Quit application
        Debug.Log("Game Quit!"); // Log for editor testing
    }
}
