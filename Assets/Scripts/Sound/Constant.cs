public class Constant
{
    public const string level = "level";


    // System Data
    public const string LAST_DAILY_REWARD_CLAIM = "LAST_DAILY_REWARD_CLAIM";
    public const string IS_START_LOOPING_DAILY_REWARD = "IS_START_LOOPING_DAILY_REWARD";
    public const string DAILY_REWARD_DAY_INDEX = "DAILY_REWARD_DAY_INDEX";
    public const string TOTAL_CLAIM_DAILY_REWARD = "TOTAL_CLAIM_DAILY_REWARD";

    // Sound
    public const string BACKGROUND_SOUND_STATE = "BACKGROUND_SOUND_STATE";
    public const string FX_SOUND_STATE = "FX_SOUND_STATE";
    public const string VIBRATE_STATE = "VIBRATE_STATE";

    // Scene
    public const string LOADING_SCENE = "Loading";
    public const string GAMEPLAY_SCENE = "GamePlay";
    public const string GAMEPLAY_SCENE_V2 = "GamePlay_V2";

    // Game Data
    public const string CURRENCY_TOTAL = "CURRENCY_TOTAL";
    public const string CURRENCY_RANK = "CURRENCY_RANK";
    public const string CHANGE_RANK = "CHANGE_RANK";
    public const string INDEX_LEVEL_CURRENT = "INDEX_LEVEL_CURRENT";
    public const string ID_SKIN_CHARACTER = "ID_SKIN_CHARACTER";
    public const string ID_SKIN_CHARACTER_OPEN = "ID_SKIN_CHARACTER_OPEN";
    public const string HINT_TOTAL = "HINT_TOTAL";

    // Shop Ski
    public const string UNLOCK_ITEM = "UNLOCK_ITEM";
    public const string CLAIM_ITEM = "CLAIM_ITEM";
    public const string EQUIP_ITEM = "EQUIP_ITEM";

    //noti
    public static string NOTIFY_ADSREWARDS = "Watch failed,Try again!";

    //firebase
    public static string Event_Start_Level = "start_level_";
    public static string Event_Win_Level = "win_level_";
    public static string Event_Fail_Level = "fail_level_";
    public static string Event_Use_Hint_Level = "use_hint_level_";
    public static string Event_Ads_Hint_Level = "use_ads_hint_level_";
    public static string Event_Open_Skin = "open_skin_";
    public const string Event_Claim_Daily_X2Hint = "claim_daily_x2_hint";
    public const string Event_Reset_Game = "reset_game_in_level_";

    public const string Event_Purchase_RemoveAds = "purchase_removeAds";
    public const string Event_Purchase_RemoveAds_Hint = "purchase_removeAds_hint";
    public const string Event_Purchase_Hint = "purchase_hint_";

    public const string Section_Open_App = "section_open_app";
    public const string Day_Goto_Game = "goto_game_day_";

    public const string Key_Open_App = "open_app_level_";
    public const string Key_Level = "level";
    public const string Key_Purchase = "purchase_iap";
    public const string Key_Skin = "skin_";
    public const string key_Day = "day_";
    public const string Key_Minutes = "minutes_";

    public const string duration = "duration_";

    public static string EventEngagement(int min)
    {
        return $"engagement_{min}_minutes";
    }

    public static string EventRate(int star)
    {
        return $"rate_{star}_star";
    }
}