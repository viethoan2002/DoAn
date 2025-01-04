using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Level
{
    [CreateAssetMenu(fileName = "Level", menuName = "Level/SpritesNode")]
    public class G4_SpriteNode : ScriptableObject
    { 
        public List<Sprite> sprites = new List<Sprite>();
    }
}
