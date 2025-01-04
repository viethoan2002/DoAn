using UnityEngine;

public static class G4_UserData
{
    public static void SetLevelLock(int index, bool value)
    {
        PlayerPrefs.SetInt($"level_{index}_lock", value ? 1 : 0);
    }

    public static bool GetLevelLock(int index)
    {
        if (index == 0)
            return false;
        else
            return PlayerPrefs.GetInt($"level_{index}_lock", 1) == 1;
    }

    public static void SetLevelDone(int index, bool value)
    {
        PlayerPrefs.SetInt($"level_{index}_done", value ? 1 : 0);
    }

    public static bool GetLevelDone(int index)
    {
        return PlayerPrefs.GetInt($"level_{index}_done", 0) == 1;
    }

    public static void SetHightScore(int amount)
    {
        PlayerPrefs.SetInt("HightScore:_", amount);
    }

    public static int GetHightScore()
    {
        return PlayerPrefs.GetInt("HightScore:_");
    }
}
