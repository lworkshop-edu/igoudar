using UnityEngine;

public static class LevelProgression
{
    private const string UnlockedLevelsPrefKey = "unlocked_levels_count";
    private const string CompletedLevelsPrefKey = "completed_levels_count";

    public static bool SaveUnlockedLevels { get; private set; } = true;

    public static void Configure(bool saveUnlockedLevels)
    {
        SaveUnlockedLevels = saveUnlockedLevels;
    }

    public static void Configure(bool unusedUnlockAllLevels, bool saveUnlockedLevels)
    {
        SaveUnlockedLevels = saveUnlockedLevels;
    }

    public static int GetUnlockedLevelsCount(int totalLevels)
    {
        if (totalLevels <= 0)
        {
            return 0;
        }

        if (!SaveUnlockedLevels)
        {
            return totalLevels;
        }

        int unlockedCount = PlayerPrefs.GetInt(UnlockedLevelsPrefKey, 1);

        unlockedCount = Mathf.Clamp(unlockedCount, 1, totalLevels);

        return unlockedCount;
    }

    public static void UnlockNextLevel(int completedLevelNumber, int totalLevels)
    {
        if (!SaveUnlockedLevels || totalLevels <= 0)
        {
            return;
        }

        int clampedCompletedLevel = Mathf.Clamp(completedLevelNumber, 0, totalLevels);
        int currentCompleted = GetCompletedLevelsCount(totalLevels);
        if (clampedCompletedLevel > currentCompleted)
        {
            PlayerPrefs.SetInt(CompletedLevelsPrefKey, clampedCompletedLevel);
        }

        int currentUnlocked = GetUnlockedLevelsCount(totalLevels);
        int targetUnlocked = Mathf.Clamp(completedLevelNumber + 1, 1, totalLevels);
        if (targetUnlocked <= currentUnlocked)
        {
            PlayerPrefs.Save();
            return;
        }

        PlayerPrefs.SetInt(UnlockedLevelsPrefKey, targetUnlocked);
        PlayerPrefs.Save();
    }

    public static int GetCompletedLevelsCount(int totalLevels)
    {
        if (totalLevels <= 0)
        {
            return 0;
        }

        if (!SaveUnlockedLevels)
        {
            return 0;
        }

        if (!PlayerPrefs.HasKey(CompletedLevelsPrefKey))
        {
            int inferredCompleted = Mathf.Clamp(GetUnlockedLevelsCount(totalLevels) - 1, 0, totalLevels);
            PlayerPrefs.SetInt(CompletedLevelsPrefKey, inferredCompleted);
            PlayerPrefs.Save();
        }

        int completedCount = PlayerPrefs.GetInt(CompletedLevelsPrefKey, 0);
        completedCount = Mathf.Clamp(completedCount, 0, totalLevels);

        return completedCount;
    }

    public static bool IsLevelCompleted(int levelNumber, int totalLevels)
    {
        if (levelNumber <= 0)
        {
            return false;
        }

        return levelNumber <= GetCompletedLevelsCount(totalLevels);
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(UnlockedLevelsPrefKey);
        PlayerPrefs.DeleteKey(CompletedLevelsPrefKey);
        PlayerPrefs.Save();
    }
}