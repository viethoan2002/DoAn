using System;
using UnityEngine;

public static partial class Data
{
    #region GAME_DATA

    public static bool ShowRate
    {
        get => PlayerPrefs.GetInt("show_rate", 0) == 1;
        set => PlayerPrefs.SetInt("show_rate", value ? 1 : 0);
    }

    public static int CurrentLevel
    {
        get { return GetInt(Constant.INDEX_LEVEL_CURRENT, 1); }

        set
        {
            SetInt(Constant.INDEX_LEVEL_CURRENT, value >= 1 ? value : 1);
            Observer.CurrentLevelChanged?.Invoke();
            ChangeRank = true;
        }
    }

    public static int CurrencyTotal
    {
        get => GetInt(Constant.CURRENCY_TOTAL, 0);
        set
        {
            Observer.SaveCurrencyTotal?.Invoke();
            SetInt(Constant.CURRENCY_TOTAL, value);
            Observer.CurrencyTotalChanged?.Invoke();
        }
    }

    public static int IdSkinCharacter
    {
        get => GetInt(Constant.ID_SKIN_CHARACTER, 1);
        set => SetInt(Constant.ID_SKIN_CHARACTER, value);
    }

    public static int IdSkinCharacterOpen
    {
        get => GetInt(Constant.ID_SKIN_CHARACTER_OPEN, 1);
        set => SetInt(Constant.ID_SKIN_CHARACTER_OPEN, value);
    }

    public static int HintTotal
    {
        get => GetInt(Constant.HINT_TOTAL, 3);
        set
        {
            SetInt(Constant.HINT_TOTAL, value);
            Observer.HintChange?.Invoke();
        }
    }

    public static int CurrentRank
    {
        get => GetInt(Constant.CURRENCY_RANK, 80000);
        set => SetInt(Constant.CURRENCY_RANK, value);
    }

    public static bool ChangeRank
    {
        get => GetBool(Constant.CHANGE_RANK, false);
        set => SetBool(Constant.CHANGE_RANK, value);
    }

    public static string LastDayOpenGame
    {
        get => GetString("LAST_DAY_OPEN_GAME", DateTime.Now.ToString());
        set => SetString("LAST_DAY_OPEN_GAME", value);
    }

    public static bool IsFirstOpenGameToday()
    {
        return (int)(DateTime.Now - DateTime.Parse(LastDayOpenGame)).TotalDays == 0;
    }

    public static int DayComeBack
    {
        get => GetInt("DAY_GOTO_GAME", 0);
        set => SetInt("DAY_GOTO_GAME", value);
    }

    #endregion

    #region SETTING_DATA

    public static bool MusicBgState
    {
        get => GetBool(Constant.BACKGROUND_SOUND_STATE, true);
        set
        {
            SetBool(Constant.BACKGROUND_SOUND_STATE, value);
            Observer.MusicChanged?.Invoke();
        }
    }

    public static bool SfxState
    {
        get => GetBool(Constant.FX_SOUND_STATE, true);
        set
        {
            SetBool(Constant.FX_SOUND_STATE, value);
            Observer.SoundChanged?.Invoke();
        }
    }

    public static bool VibrateState
    {
        get => GetBool(Constant.VIBRATE_STATE, true);
        set => SetBool(Constant.VIBRATE_STATE, value);
    }

    #endregion

    #region DAILY_REWARD

    public static bool IsClaimedTodayDailyReward()
    {
        return (int)(DateTime.Now - DateTime.Parse(LastDailyRewardClaimed)).TotalDays == 0;
    }

    public static bool IsStartLoopingDailyReward
    {
        get => PlayerPrefs.GetInt(Constant.IS_START_LOOPING_DAILY_REWARD, 0) == 1;
        set => PlayerPrefs.SetInt(Constant.IS_START_LOOPING_DAILY_REWARD, value ? 1 : 0);
    }

    public static int DailyRewardDayIndex
    {
        get => GetInt(Constant.DAILY_REWARD_DAY_INDEX, 1);
        set => SetInt(Constant.DAILY_REWARD_DAY_INDEX, value);
    }

    public static string LastDailyRewardClaimed
    {
        get => GetString(Constant.LAST_DAILY_REWARD_CLAIM, DateTime.Now.AddDays(-1).ToString());
        set => SetString(Constant.LAST_DAILY_REWARD_CLAIM, value);
    }

    public static int TotalClaimDailyReward
    {
        get => GetInt(Constant.TOTAL_CLAIM_DAILY_REWARD, 0);
        set => SetInt(Constant.TOTAL_CLAIM_DAILY_REWARD, value);
    }

    #endregion

    #region ShopSkin

    public static bool IsItemEquipped(string itemIdentity)
    {
        return GetBool($"{Constant.EQUIP_ITEM}_{IdItemUnlocked}");
    }

    public static void SetItemEquipped(string itemIdentity, bool isEquipped = true)
    {
        SetBool($"{Constant.EQUIP_ITEM}_{IdItemUnlocked}", isEquipped);
    }

    public static string IdItemUnlocked = "";

    public static bool IsItemUnlocked
    {
        get => GetBool($"{Constant.UNLOCK_ITEM}_{IdItemUnlocked}");
        set => SetBool($"{Constant.UNLOCK_ITEM}_{IdItemUnlocked}", value);
    }

    public static bool IsItemClaimed
    {
        get => GetBool($"{Constant.CLAIM_ITEM}_{IdItemUnlocked}");
        set => SetBool($"{Constant.CLAIM_ITEM}_{IdItemUnlocked}", value);
    }

    #endregion


    #region remote config

    public static int inter_ad_capping_time
    {
        get => PlayerPrefs.GetInt("inter_ad_capping_time", 12);
        set => PlayerPrefs.SetInt("inter_ad_capping_time", value);
    }

    public static int open_ad_capping_time
    {
        get => PlayerPrefs.GetInt("open_ad_capping_time", 20);
        set => PlayerPrefs.SetInt("open_ad_capping_time", value);
    }

    public static int collapsible_banner_capping_time
    {
        get => PlayerPrefs.GetInt("collapsible_banner_capping_time", 30);
        set => PlayerPrefs.SetInt("collapsible_banner_capping_time", value);
    }

    public static int collapsible_banner_life_time
    {
        get => PlayerPrefs.GetInt("collapsible_banner_life_time", 8);
        set => PlayerPrefs.SetInt("collapsible_banner_life_time", value);
    }

    public static bool open_ad_on_off
    {
        get => PlayerPrefs.GetInt("open_ad_on_off", 1) == 1;
        set => PlayerPrefs.SetInt("open_ad_on_off", value ? 1 : 0);
    }

    public static bool banner_ad_on_off
    {
        get => PlayerPrefs.GetInt("banner_ad_on_off", 1) == 1;
        set => PlayerPrefs.SetInt("banner_ad_on_off", value ? 1 : 0);
    }

    public static bool collap_ad_on_off
    {
        get => PlayerPrefs.GetInt("banner_collap_ad_on_off", 1) == 1;
        set => PlayerPrefs.SetInt("banner_collap_ad_on_off", value ? 1 : 0);
    }

    public static bool inter_ad_on_off
    {
        get => PlayerPrefs.GetInt("inter_ad_on_off", 1) == 1;
        set => PlayerPrefs.SetInt("inter_ad_on_off", value ? 1 : 0);
    }

    public static int level_show_rate
    {
        get => PlayerPrefs.GetInt("level_show_rate", 1);
        set => PlayerPrefs.SetInt("level_show_rate", value);
    }

    public static string open_ad_id
    {
        get => PlayerPrefs.GetString("open_ad_id", "ca-app-pub-2913496970595341/7554145632");
        set => PlayerPrefs.SetString("open_ad_id", value);
    }

    public static string collap_ad_id
    {
        get => PlayerPrefs.GetString("collap_ad_id", "ca-app-pub-2913496970595341/9798570568");
        set => PlayerPrefs.SetString("collap_ad_id", value);
    }

    // public static string levels
    // {
    //     get => PlayerPrefs.GetString("levels",
    //         "23,1,2,42,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103");//,104,105,106,107,108,109,110,111,112,113,114");
    //     set => PlayerPrefs.SetString("levels", value);
    // }

    public static string levels2
    {
        get => PlayerPrefs.GetString("levels2",
            "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114");
        set => PlayerPrefs.SetString("levels2", value);
    }

    #endregion
}

public static partial class Data
{
    private static bool GetBool(string key, bool defaultValue = false) =>
        PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) > 0;

    private static void SetBool(string id, bool value) => PlayerPrefs.SetInt(id, value ? 1 : 0);

    private static int GetInt(string key, int defaultValue) => PlayerPrefs.GetInt(key, defaultValue);
    private static void SetInt(string id, int value) => PlayerPrefs.SetInt(id, value);

    private static string GetString(string key, string defaultValue) => PlayerPrefs.GetString(key, defaultValue);
    private static void SetString(string id, string value) => PlayerPrefs.SetString(id, value);
}