using System;
using _Camera;
using UnityEngine;

namespace Game0.Game_03.Scripts.Gameplay
{
    public class G3_PopupCtrl : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = CameraManager.Instance.GetUICamera();
        }
    }
}
