using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class BackgroundManager : SingletonDontDestroy<BackgroundManager>
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Sprite defaultBackground;

        public void ActiveBackground(bool en)
        {
            backgroundImage.enabled = en;
        }

        public void SetDefaultBackground()
        {
            backgroundImage.enabled = true;
            backgroundImage.sprite = defaultBackground;
        }

        public void SetupBackGroundImage(Sprite backgroundSprite)
        {
            backgroundImage.sprite = backgroundSprite;
        }
    }
}
