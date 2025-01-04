using UnityEngine;

namespace Gameplay
{
    public class G3_BaseNode : MonoBehaviour
    {
        [SerializeField] private G3_DirectionType curDir;

        public void SetDirection(G3_DirectionType dir)
        {
            curDir = dir;
        }

        public G3_DirectionType GetDirection()
        {
            return curDir;
        }

        public Vector2 GetVectorByOrder(G3_DirectionType order)
        {
            Vector2 dir = Vector2.zero;
            switch (order)
            {
                case G3_DirectionType.Up:
                    dir = transform.up;
                    break;
                case G3_DirectionType.Right:
                    dir = transform.right;
                    break;
                case G3_DirectionType.Down:
                    dir = -transform.up;
                    break;
                case G3_DirectionType.Left:
                    dir = -transform.right;
                    break;
            }

            return dir;
        }

        public G3_DirectionType GetDirectionByOrder(G3_DirectionType order)
        {
            int indexDir = ((int)curDir + (int)order) % 4;

            return (G3_DirectionType)indexDir;
        }
    }
}
