using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MiniGame
{
    [Serializable]
    public struct GameData
    {
        public AssetReference miniGameAsset;
        public string nameGame;
        public bool useBackground;
        public Sprite backgroundSprite;
    }
    
    [CreateAssetMenu(fileName = "MiniGameData", menuName = "MiniGame/MiniGameData")]
    public class MiniGameData : ScriptableObject
    {
        public List<GameData> GameData = new List<GameData>();
    }
}