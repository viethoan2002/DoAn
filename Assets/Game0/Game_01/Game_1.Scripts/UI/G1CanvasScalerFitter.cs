using UnityEngine;
using UnityEngine.UI;

namespace Game_01.Game_1.Scripts.UI
{
    public class G1CanvasScalerFitter : MonoBehaviour
    {
        void Start()
        {
            var canvas = GetComponent<CanvasScaler>();
        
            float targetRatio = 1280f / 720;
            float screenRatio = Screen.width / (float)Screen.height;
        
            canvas.matchWidthOrHeight = (screenRatio >= targetRatio) ? 1 : 0;
        }
    }
}