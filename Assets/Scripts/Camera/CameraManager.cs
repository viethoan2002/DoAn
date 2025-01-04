using DG.Tweening;
using UnityEngine;

namespace _Camera
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera uiCamera;

        private void Awake()
        {
            Instance = this;
        }

        public Camera GetMainCamera()
        {
            return mainCamera;
        }

        public Camera GetUICamera()
        {
            return uiCamera;
        }

        public void SetupDefaultCamera()
        {
            
        }

        public void CameraZoomIn()
        {
            DOTween.To(() => uiCamera.orthographicSize, x => uiCamera.orthographicSize = x, 4.25f, .5f);
            DOTween.To(() => uiCamera.orthographicSize, x => mainCamera.orthographicSize = x, 4.25f, .5f);
        }

        public void CameraZoomOut()
        {
            DOTween.To(() => uiCamera.orthographicSize, x => uiCamera.orthographicSize = x, 5.5f, .5f);
            DOTween.To(() => uiCamera.orthographicSize, x => mainCamera.orthographicSize = x, 5.5f, .5f);
        }
    }
}
