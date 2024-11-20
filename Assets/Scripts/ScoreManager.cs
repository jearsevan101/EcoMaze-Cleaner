using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton instance

    [Header("Scoring")]
    private int currentScore;
    private int remainingTrash;
    private int totalTrash;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    [SerializeField] private TextMeshProUGUI trashRemaining;
    [SerializeField] private TextMeshProUGUI timerDisplay;

    [Header("Game Settings")]
    [SerializeField] private float gameTime = 12f;
    [SerializeField] private GameObject trashParent;

    private float currentTime;
    private bool isGameRunning = true;

    private void Awake()
    {
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
        }
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
        if (currentScore > (5 * 70 * totalTrash / 100))
        {
            // Update both timerDisplay and scoreDisplay
            timerDisplay.text = "Time's Up!";
            scoreDisplay.text = "You Win! Final Score: " + currentScore;
            Debug.Log("You Win! Final Score: " + currentScore);
        }
        else
        {
            // Update both timerDisplay and scoreDisplay
            timerDisplay.text = "Time's Up!";
            scoreDisplay.text = "You Lose! Final Score: " + currentScore;
            Debug.Log("You Lose! Final Score: " + currentScore);
        }
        Debug.Log("Game Over! Final Score: " + currentScore);
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