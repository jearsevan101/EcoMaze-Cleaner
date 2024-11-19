using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton instance

    private int currentScore;
    [SerializeField] private TextMeshProUGUI scoreDisplay;

    private void Awake()
    {
        // Ensure only one instance exists
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
        // Initialize the score display at the start
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        Debug.Log($"Score Updated: {currentScore}");
        UpdateScoreDisplay();
    }

    public int GetScore()
    {
        return currentScore;
    }
    private void UpdateScoreDisplay()
    {
        if (scoreDisplay != null)
        {
            scoreDisplay.text = "Score: " + currentScore.ToString();  // Update the UI text
        }
    }
}
