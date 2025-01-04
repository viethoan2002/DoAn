using System;
using UnityEngine;

public static class Observer
{
    #region GameSystem

    // Debug
    public static Action DebugChanged;

    // Currency
    public static Action SaveCurrencyTotal;
    public static Action CurrencyTotalChanged;

    public static Action<Vector3> SetStartFromCoinMove;

    //Hint
    public static Action HintChange;

    // Level Spawn
    public static Action CurrentLevelChanged;

    // Setting
    public static Action MusicChanged;
    public static Action SoundChanged;

    public static Action VibrationChanged;

    #endregion

    #region Gameplay

    //Btn Color
    public static Action<bool> OffOutline;

    // paint
    public static Action Paint;
    public static Action PaintHint;

    //shop
    public static Action ChangeSkin;

    //Notificaton
    public static Action<String> Notify;
    //shop Iap

    #endregion
}