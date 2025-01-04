using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundConfig", menuName = "ScriptableObject/SoundConfig")]
public class SoundConfig : ScriptableObject
{
    public List<SoundData> SoundDatas;

    public AudioClip GetAudioClipByType(SoundType soundType)
    {
        for (int i = 0; i < SoundDatas.Count; i++)
        {
            if (SoundDatas[i].soundType == soundType)
            {
                return SoundDatas[i].audioClip;
            }
        }

        return null;
    }
}

[Serializable]
public class SoundData
{
    public SoundType soundType;

    public AudioClip audioClip;
}

public enum SoundType
{
    Button,
    ButtonColor,
    NewPaintMap,
    PaintInGame,
    PaintFlagWin,
    PaintFlagFail,
    Back,
    Hint,
    DrumRoll,
    Gift,
    GiftOld,
    GiftAppear,
    GiftAppearOld,
    GiftAppearOld2,
    GiftClaim,
    GiftUnboxing,
    Harp1,
    Harp2,
    Point,
    PurchaseSuccess,
    SkinClaimed,
    SkinOpen,
    SkinUnlockIntro,
    StoreOpen,
    Success,
}