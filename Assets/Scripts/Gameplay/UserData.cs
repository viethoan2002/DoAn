using UnityEngine;

public static class UserData
{
    public static void SetLevelLock(int indexGame,int indexLevel, bool value)
    {
        PlayerPrefs.SetInt($"Game{indexGame}_level_{indexLevel}_lock", value ? 1 : 0);
    }

    public static bool GetLevelLock(int indexGame,int indexLevel)
    {
        if (indexLevel == 0)
            return false;
        else
            return PlayerPrefs.GetInt($"Game{indexGame}_level_{indexLevel}_lock", 1) == 1;
    }

    public static void SetLevelDone(int indexGame,int indexLevel, bool value)
    {
        PlayerPrefs.SetInt($"Game{indexGame}_level_{indexLevel}_done", value ? 1 : 0);
    }

    public static bool GetLevelDone(int indexGame,int indexLevel)
    {
        return PlayerPrefs.GetInt($"Game{indexGame}_level_{indexLevel}_done", 0) == 1;
    }

    public static void SetLevelPlay(int indexGame, int indexLevel)
    {
        PlayerPrefs.SetInt($"Game{indexGame}_level_play", indexLevel);
    }

    public static int GetLevelPlay(int indexGame)
    {
        return PlayerPrefs.GetInt($"Game{indexGame}_level_play", 0);
    }
}
