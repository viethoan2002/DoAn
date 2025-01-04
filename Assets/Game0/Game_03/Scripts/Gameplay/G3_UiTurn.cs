using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class G3_UiTurn : MonoBehaviour
    {
        [SerializeField] private Text txtScore;

        public void UpdateScore(int score)
        {
            txtScore.color = score > 0 ? Color.black : Color.red;
            txtScore.text = score.ToString();
            txtScore.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    txtScore.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear);
                });
        }
    }
}
