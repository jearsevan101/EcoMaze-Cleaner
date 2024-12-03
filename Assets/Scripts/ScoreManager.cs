using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton instance
    [SerializeField] private int currentLevel;

    [Header("Scoring")]
    private int currentScore;
    private int remainingTrash;
    private int totalTrash;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private TextMeshProUGUI trashRemaining;
    [SerializeField] private TextMeshProUGUI timerDisplay;


    [SerializeField] private PopUpUI popUpFinished;

    [Header("Game Settings")]
    [SerializeField] private float gameTime;
    [SerializeField] private GameObject trashParent;

    private float currentTime;
    private bool isGameRunning = true;

    private void Awake()
    {
        popUpFinished.gameObject.SetActive(false);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentTime = gameTime;
        UpdateScoreDisplay();
        UpdateRemainingTrash();
        totalTrash = remainingTrash;
    }

    private void Update()
    {
        if (isGameRunning)
        {
            currentTime -= Time.deltaTime;

            // Prevent negative time
            if (currentTime <= 0)
            {
                currentTime = 0;
                EndGame();
            }

            UpdateTimerDisplay();
            if (remainingTrash == 0)
            {
                EndGame();
            }
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    private void OnLevelComplete()
    {
        // Unlock the next level and save current score and time
        LevelManager.Instance.UnlockNextLevel(currentLevel, currentScore, currentTime);
    }
    private void UpdateTimerDisplay()
    {
        if (timerDisplay != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerDisplay.text = string.Format("{0:00}:{1:00}", Mathf.Abs(minutes), Mathf.Abs(seconds));
        }
    }

    private void EndGame()
    {
        isGameRunning = false;
        popUpFinished.gameObject.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        // Update timerDisplay and scoreDisplay
        timerDisplay.text = "Time's Up!";
        int minimumScore = 5 * 70 * totalTrash / 100;
        scoreDisplay.text = currentScore > minimumScore
            ? "You Win! Final Score: " + currentScore
            : "You Lose! Final Score: " + currentScore;
        if (currentScore > minimumScore)
        {
            popUpFinished.updateScore(GetScore(), minimumScore);
            popUpFinished.updateSuccessInfo(true);
            OnLevelComplete();
        }
        else
        {
            popUpFinished.updateScore(GetScore(), minimumScore);
            popUpFinished.updateSuccessInfo(false);
        }
        Debug.Log(currentScore > minimumScore
            ? "You Win! Final Score: " + currentScore
            : "You Lose! Final Score: " + currentScore);
    }


    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreDisplay();
        UpdateRemainingTrash();
    }

    public int GetScore()
    {
        return currentScore;
    }

    private void UpdateScoreDisplay()
    {
        if (scoreDisplay != null)
        {
            scoreDisplay.text = "Score: " + currentScore.ToString();
        }
    }

    private void UpdateRemainingTrash()
    {
        remainingTrash = trashParent.transform.childCount;
        trashRemaining.text = "Trash Remaining: " + remainingTrash.ToString();
    }
}