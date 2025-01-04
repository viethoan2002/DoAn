using Game0.Game_01.Game_1.Scripts.Gameplay.Block;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game_01.Game_1.Scripts.Gameplay.Block
{
    public class G1MiniBlock : MonoBehaviour
    {
        [FormerlySerializedAs("block")] public G1BlockCtrl g1Block;
        [FormerlySerializedAs("_spriteRenderer")] [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite baseSprite, radiusSprite, circleSprite;
        [SerializeField] private GameObject wrong;
        [FormerlySerializedAs("IsHead")] public bool isHead;

        private void SetupSprite(int indexSprite)
        {
            
        }

        public void SetColor(bool en)
        {
            spriteRenderer.color = en ? new Color(1, 1, 1, 1) : new Color(.7f, .7f, .7f, 1);
        }

        public void SetSpriteCanPush(bool en)
        {
            spriteRenderer.enabled = en;
            wrong.SetActive(!en);
        }

        public void SetSpriteCircle()
        {
            spriteRenderer.sprite = circleSprite;
        }

        public void SetSpriteBase()
        {
            spriteRenderer.sprite = baseSprite;
        }

        public void SetSpriteByNeighbor(Vector3 neighbor1, Vector3 neighbor2)
        {
            spriteRenderer.transform.up=Vector3.up;
            spriteRenderer.transform.right = Vector3.right;
            
            if (Mathf.Approximately(Mathf.Round(neighbor1.y * 100000f) / 100000f,Mathf.Round( neighbor2.y * 100000f) / 100000f))
            {
                spriteRenderer.sprite = baseSprite;
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                return;
            } 
            
            if(Mathf.Approximately(Mathf.Round(neighbor1.x * 100000f) / 100000f,Mathf.Round( neighbor2.x * 100000f) / 100000f))
            {
                spriteRenderer.sprite = baseSprite;
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
                return;
            }
                

            spriteRenderer.sprite = radiusSprite;
            if (Mathf.Approximately(Mathf.Round(neighbor1.x * 100000f) / 100000f,Mathf.Round(transform.position.x * 100000f) / 100000f ))
            {
                spriteRenderer.transform.localScale = new Vector3(neighbor2.x < transform.position.x ? -1 : 1,
                    neighbor1.y < transform.position.y ? 1 : -1,
                    1);
            }
            else
            {
                spriteRenderer.transform.localScale = new Vector3(neighbor1.x < transform.position.x ? -1 : 1,
                    neighbor2.y < transform.position.y ? 1 : -1,
                    1);
            }
        }

        private void Reset()
        {
            g1Block = GetComponentInParent<G1BlockCtrl>();
        }
    }
}
