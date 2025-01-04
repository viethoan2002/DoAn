using System;
using Gameplay.Level;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Map
{
    public class G4_BaseNode : MonoBehaviour
    {
        [FormerlySerializedAs("spriteData")] [FormerlySerializedAs("spriteNode")] [SerializeField] private G4_SpriteNode g4SpriteData;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private int indexSprite;

        private void OnValidate()
        {
            if(spriteRenderer != null)
                spriteRenderer.sprite = g4SpriteData.sprites[indexSprite];
        }
    }
}
