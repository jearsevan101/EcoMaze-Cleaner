using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI successText;
    [SerializeField] private TextMeshProUGUI scoreText;


    private void Awake()
    {
        retryButton.onClick.AddListener(() =>
        {
            // retry
            Loader.RetryGame();
            this.gameObject.SetActive(false);
        });
        nextLevelButton.onClick.AddListener(() =>
        {
            if (ScoreManager.Instance != null)
            {
                // Access the current level
                int currentLevel = ScoreManager.Instance.GetCurrentLevel();
                Loader.LoadString($"Level{currentLevel+1}");
            }
            else
            {
                Debug.LogError("ScoreManager instance not found!");
            }

        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    public void updateScore(int Score)
    {
        scoreText.text = "Total Score: " + Score.ToString();    
    }
    public void updateSuccessInfo(bool isWinning)
    {
        if (isWinning)
        {
            successText.text = "Success";
            nextLevelButton.interactable = true;
        }
        else
        {
            successText.text = "Failed";
            nextLevelButton.interactable = false;
        }
    }
}
