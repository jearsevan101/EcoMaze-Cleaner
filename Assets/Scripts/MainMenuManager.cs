using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject scrollView; // Reference to the ScrollView GameObject
    [SerializeField] private List<Button> levelButtons;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        int unlockedLevel = LevelManager.Instance.GetUnlockedLevel();
        for (int i = 0; i < levelButtons.Count; i++)
        {
            // Get level data for each button
            var (score, time) = LevelManager.Instance.GetLevelData(i);

            // Set button interactability
            if (i <= unlockedLevel)
            {
                levelButtons[i].interactable = true;
            }
            else
            {
                levelButtons[i].interactable = false;
            }

            // Find the ButtonLevel component on the button or its children
            ButtonLevel buttonLevel = levelButtons[i].transform.parent.GetComponent<ButtonLevel>();
            if (buttonLevel != null)
            {
                // Update score and time display
                buttonLevel.updateScore(score, Mathf.FloorToInt(time));
            }
        }
       

        // Add listeners to buttons for loading levels
        for (int i = 0; i < levelButtons.Count; i++)
        {
            int levelIndex = i; // Levels start from 0
            levelButtons[i].onClick.AddListener(() =>
            {
                LoadLevel(levelIndex);
            });
        }
    }

    private void LoadLevel(int levelIndex)
    {
        // Load the level scene
        Debug.Log($"Loading Level {levelIndex}...");
        // Replace "LevelX" with your level naming scheme
        Loader.LoadString($"Level{levelIndex}");
    }
    private void Awake()
    {
        // Disable the ScrollView initially
        scrollView.SetActive(false);

        startButton.onClick.AddListener(() =>
        {
            scrollView.SetActive(!scrollView.activeSelf);
        });

        tutorialButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.Tutorial);
        });

        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}