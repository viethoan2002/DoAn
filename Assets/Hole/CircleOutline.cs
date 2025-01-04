using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CircleOutline : MonoBehaviour
    {
        public static CircleOutline Instance;
        [SerializeField] private Canvas canvas;
        private float _witdh, _height;
        [SerializeField] private RectTransform circleOutline;
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void ScaleIn(Action action = null)
        {
            circleOutline.anchoredPosition = Vector2.zero;
            circleOutline.localScale = new Vector3(15, 15, 15);
            circleOutline.DOScale(Vector3.zero, 1.5f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                action?.Invoke();
            });
        }


        public void ScaleOut(Action action = null)
        {
            circleOutline.anchoredPosition = Vector2.zero;
            circleOutline.localScale = Vector3.zero;
            DOVirtual.DelayedCall(1f, () =>
            {
                circleOutline.DOScale(new Vector3(15, 15, 15), 1.5f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
                {
                    action?.Invoke();
                });
            });
        }
    }
}
