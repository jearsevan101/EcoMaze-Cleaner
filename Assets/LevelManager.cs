using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private const string UnlockedLevelKey = "UnlockedLevel";
    private const string LevelScoreKey = "LevelScore_"; // Prefix for scores
    private const string LevelTimeKey = "LevelTime_";   // Prefix for times

    private int unlockedLevel = 0;

    private void Awake()
    {
        // Singleton pattern to persist across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load unlocked level from saved data
            unlockedLevel = PlayerPrefs.GetInt(UnlockedLevelKey, 1);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetUnlockedLevel()
    {
        return unlockedLevel;
    }

    public (int score, float time) GetLevelData(int level)
    {
        int score = PlayerPrefs.GetInt(LevelScoreKey + level, 0); // Default score is 0
        float time = PlayerPrefs.GetFloat(LevelTimeKey + level, 0f); // Default time is 0
        return (score, time);
    }

    public void UnlockNextLevel(int currentLevel, int score, float time)
    {
        if (currentLevel < 0 || currentLevel >= unlockedLevel)
        {
            Debug.LogError("Invalid level or level not unlocked yet.");
            return;
        }

        // Save score and time for the current level
        PlayerPrefs.SetInt(LevelScoreKey + currentLevel, score);
        PlayerPrefs.SetFloat(LevelTimeKey + currentLevel, time);

        // Unlock the next level if it isn't already unlocked
        if (unlockedLevel <= currentLevel)
        {
            unlockedLevel = currentLevel + 1;
            PlayerPrefs.SetInt(UnlockedLevelKey, unlockedLevel);
        }

        PlayerPrefs.Save();
    }

    public void ResetProgress()
    {
        unlockedLevel = 1;
        PlayerPrefs.SetInt(UnlockedLevelKey, unlockedLevel);

        // Clear all saved scores and times
        for (int i = 1; i <= 100; i++) // Assuming max 100 levels
        {
            PlayerPrefs.DeleteKey(LevelScoreKey + i);
            PlayerPrefs.DeleteKey(LevelTimeKey + i);
        }

        PlayerPrefs.Save();
    }
}
