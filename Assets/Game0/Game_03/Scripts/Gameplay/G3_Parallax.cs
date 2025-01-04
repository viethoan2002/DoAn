using UnityEngine;

namespace Gameplay
{
    public class G3_Parallax : MonoBehaviour
    {
        [SerializeField] private Vector3 startPos;

        [SerializeField] private Transform startTrans;
        [SerializeField] private Transform endTrans;
        [SerializeField] private Vector2 direction;

        [SerializeField] private float repeatWidth;

        [SerializeField] private float speed;
        // Start is called before the first frame update
        void Start()
        {
            startPos = transform.position;
            repeatWidth = -(endTrans.position-startTrans.position).x;
            direction=(endTrans.position-startTrans.position).normalized;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(direction * Time.deltaTime * speed);
            if (transform.position.x < startPos.x - repeatWidth)
            {
                transform.position = startPos;
            }
        }
    }
}
