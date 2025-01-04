using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class G3_BodyNode : G3_BaseNode
    {
        public SpriteRenderer renderer;
        public Sprite bodyHorizontal, bodyVertical;
        [FormerlySerializedAs("left_up")] public Sprite leftUp;
        [FormerlySerializedAs("left_down")] public Sprite leftDown;
        [FormerlySerializedAs("right_up")] public Sprite rightUp;
        [FormerlySerializedAs("right_down")] public Sprite rightDown;

        public void SetupSprite(G3_DirectionType curDir,G3_DirectionType targetDir)
        {
            switch (curDir)
            {
                case G3_DirectionType.Up:
                    switch (targetDir)
                    {
                        case G3_DirectionType.Left:
                            renderer.sprite = rightUp;
                            break;
                        case G3_DirectionType.Right:
                            renderer.sprite = leftUp;
                            break;
                        case G3_DirectionType.Up:
                            renderer.sprite = bodyVertical;
                            break;
                    }
                    break;
                case G3_DirectionType.Right:
                    switch (targetDir)
                    {
                        case G3_DirectionType.Down:
                            renderer.sprite = rightUp;
                            break;
                        case G3_DirectionType.Right:
                            renderer.sprite = bodyHorizontal;
                            break;
                        case G3_DirectionType.Up:
                            renderer.sprite = rightDown;
                            break;
                    }
                    break;
                case G3_DirectionType.Down:
                    switch (targetDir)
                    {
                        case G3_DirectionType.Left:
                            renderer.sprite = rightDown;
                            break;
                        case G3_DirectionType.Right:
                            renderer.sprite = leftDown;
                            break;
                        case G3_DirectionType.Down:
                            renderer.sprite = bodyVertical;
                            break;
                    }
                    break;
                case G3_DirectionType.Left:
                    switch (targetDir)
                    {
                        case G3_DirectionType.Left:
                            renderer.sprite = bodyHorizontal;
                            break;
                        case G3_DirectionType.Down:
                            renderer.sprite = leftUp;
                            break;
                        case G3_DirectionType.Up:
                            renderer.sprite = leftDown;
                            break;
                    }
                    break;
            }
        }

        public void SetupSpriteByPos(Transform trans)
        {
            if (trans.position.x > transform.position.x)
            {
                SetupSprite(G3_DirectionType.Up, G3_DirectionType.Right);
            }
            else if (trans.position.x < transform.position.x)
            {
                SetupSprite(G3_DirectionType.Up, G3_DirectionType.Left);
            }
            else if (trans.position.y > transform.position.y)
            {
                SetupSprite(G3_DirectionType.Up, G3_DirectionType.Up);
            }
            else
            {
                SetupSprite(G3_DirectionType.Up, G3_DirectionType.Down);
            }
        }
    }
}
