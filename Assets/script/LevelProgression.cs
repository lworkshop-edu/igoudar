using UnityEngine;

public static class LevelProgression
{
    private const string UnlockedLevelsPrefKey = "unlocked_levels_count";

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

        int currentUnlocked = GetUnlockedLevelsCount(totalLevels);
        int targetUnlocked = Mathf.Clamp(completedLevelNumber + 1, 1, totalLevels);
        if (targetUnlocked <= currentUnlocked)
        {
            return;
        }

        PlayerPrefs.SetInt(UnlockedLevelsPrefKey, targetUnlocked);
        PlayerPrefs.Save();
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(UnlockedLevelsPrefKey);
        PlayerPrefs.Save();
    }
}