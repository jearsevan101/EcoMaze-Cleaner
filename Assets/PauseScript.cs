using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI; // Drag & drop Pause Menu Canvas here
    public GameObject backgroundBlur; // Drag & drop semi-transparent background image here

    private bool isPaused = false;

    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;


    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //resumeButton.onClick.AddListener(ResumeGame);
        //restartButton.onClick.AddListener(RestartGame);
        //quitButton.onClick.AddListener(QuitGame);
    }

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
        Cursor.visible = true; // Show cursor
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Debug.Log("Game Paused!");
        Time.timeScale = 0f; // Freeze game


        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        backgroundBlur.SetActive(false);

        Time.timeScale = 1f; // Resume game

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("BUTTON RESUME GAME PRESSED");

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
        Loader.Load(Loader.Scene.MainMenu); // Load Main Menu scene
        Debug.Log("Game Quit!"); // Log for editor testing
    }

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            ResumeGame();
        });

        restartButton.onClick.AddListener(RestartGame);

        quitButton.onClick.AddListener(QuitGame);
    }
}
