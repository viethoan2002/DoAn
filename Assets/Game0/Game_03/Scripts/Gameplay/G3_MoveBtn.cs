using System;
using Gameplay;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class G3_MoveBtn : MonoBehaviour
    {
        [SerializeField] private Button button;
        [FormerlySerializedAs("handType")] [SerializeField] private G3_HandType g3HandType;
        [FormerlySerializedAs("directionType")] [SerializeField] private G3_DirectionType g3DirectionType;
        public static Action<G3_HandType, G3_DirectionType> OnMove;

        private void Awake()
        {
            button.onClick.AddListener(() =>
            {
                OnMove?.Invoke(g3HandType, g3DirectionType);
            });
        }
    }
}
