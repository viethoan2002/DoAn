using System.Collections.Generic;
using DG.Tweening;
using Game_01.Game_1.Scripts.Gameplay.Block;
using Game_01.Game_1.Scripts.Gameplay.Level;
using Gameplay;
using Raycast;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game0.Game_01.Game_1.Scripts.Gameplay.Block
{
    public enum BlockSate
    {
        InBoard =0,
        IsDragging = 1,
        InMap = 2
    }
    public class G1BlockCtrl : MonoBehaviour,IClickHandle
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private List<G1MiniBlock> miniBlocks = new List<G1MiniBlock>();
        
        [SerializeField] private Transform parentTransform;
        [SerializeField] private Vector3 localPositionInParent;
        [SerializeField] private Vector3 localAngleInParent;
        
        private Vector2 _originalPosition;
        private float _minDistanceDrag;
        
        [FormerlySerializedAs("_currentSate")] public BlockSate currentSate;
        private bool _isRotating;

        private void Awake()
        {
            parentTransform = transform.parent;
            localPositionInParent = transform.localPosition;
            localAngleInParent = transform.localEulerAngles;
        }

        private void OnEnable()
        {
            playerCamera = MainManager.Instance.mainCamera;
            currentSate = BlockSate.InBoard;
        }

        private void GetInBoard()
        {
            SetupSpriteMini();
            transform.parent = parentTransform;
            transform.DOLocalRotate(localAngleInParent, 0.25f);
            transform.DOLocalMove(localPositionInParent, 0.25f).SetEase(Ease.Flash).OnComplete(() =>
            {
                foreach (var block in miniBlocks)
                {
                    block.SetSpriteCanPush(true);
                    currentSate = BlockSate.InBoard;
                }
            });
        }

        private void GetInMap()
        {
            var node = G1PathFinding.Instance.GetGrid().GetPathNode(transform.position);
            transform.DOMove(G1PathFinding.Instance.GetGrid().GetWorldPosition(node.X, node.Y), 0.05f)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    foreach (var t in miniBlocks)
                    {
                        t.SetColor(false);
                        node = G1PathFinding.Instance.GetGrid().GetPathNode(t.transform.position);
                        node.IsWalkable = false;
                        node.G1MiniBlock = t;
                    }

                    currentSate = BlockSate.InMap;
                    G1GameManager.Instance.curG1LevelCtrl.SetupPath();
                });
        }

        private void GetOutOfMap()
        {
            //G1PopupCtrl.Instance.GetPopupByType<G1PopupGameplay>().DeActiveAccelerator();
            
            foreach (var t in miniBlocks)
            {
                var node=G1PathFinding.Instance.GetGrid().GetPathNode(t.transform.position);
                t.SetColor(true);
                if (node != null)
                {
                    node.IsWalkable = true;
                    node.G1MiniBlock = null;
                }
            }
            
            SetupSpriteMini();
        }

        private void OnMouseDown()
        {
            /*_originalPosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            G1GameManager.Instance.hand.transform.position = _originalPosition;
            
            if(currentSate == BlockSate.InMap)
                GetOutOfMap();*/
        }

        private void OnMouseUp()
        {
            /*switch (currentSate)
            {
                case BlockSate.InBoard:
                    var parent = transform.parent;
                    if (!_isRotating)
                    {
                        _isRotating = true;
                        parent.DORotate(new Vector3(parent.eulerAngles.x, parent.eulerAngles.y, parent.eulerAngles.z + 90),
                            0.25f).SetEase(Ease.Flash).OnComplete(() =>
                        {
                            _isRotating = false;
                        });
                    }
                    break;
                case BlockSate.IsDragging:
                    transform.parent = G1GameManager.Instance.curG1LevelCtrl.transform;
                    if(!CanAddToMap())
                        GetInBoard();
                    else
                        GetInMap();
                    break;
                case BlockSate.InMap:
                    GetInMap();
                    break;
            }*/
        }

        private void OnMouseDrag()
        {
            /*if (Vector2.Distance(_originalPosition, playerCamera.ScreenToWorldPoint(Input.mousePosition)) >
                _minDistanceDrag)
            {
                currentSate = BlockSate.IsDragging;
                transform.parent = G1GameManager.Instance.hand;
                G1GameManager.Instance.curG1LevelCtrl.SetupPath();
            }


            if (currentSate == BlockSate.IsDragging)
            {
                var worldPoint = playerCamera.ScreenToWorldPoint(Input.mousePosition);
                G1GameManager.Instance.hand.position = new Vector3(worldPoint.x, worldPoint.y, -1);
                UpdateMiniBlocks();
            }*/
        }

        private void UpdateMiniBlocks()
        {
            foreach (var t in miniBlocks)
            {
                var node = G1PathFinding.Instance.GetGrid().GetPathNode(t.transform.position);
                if(node == null || !node.IsWalkable)
                    t.SetSpriteCanPush(false);
                else if (!t.isHead && node.IsStation)
                    t.SetSpriteCanPush(false);
                else
                    t.SetSpriteCanPush(true);
            }
        }

        private bool CanAddToMap()
        {
            foreach (var t in miniBlocks)
            {
                var node = G1PathFinding.Instance.GetGrid().GetPathNode(t.transform.position);
                if (node == null || !node.IsWalkable)
                    return false;

                if (!t.isHead && node.IsStation)
                    return false;
            }
            
            return true;
        }

        private void SetupSpriteMini()
        {
            for (int i = 0; i < miniBlocks.Count; i++)
            {
                if (i == 0 || i == miniBlocks.Count - 1)
                {
                    miniBlocks[i].SetSpriteBase();
                }
                else
                {
                    miniBlocks[i].SetSpriteByNeighbor(miniBlocks[i - 1].transform.position,
                        miniBlocks[i + 1].transform.position);
                }
            }
        }

        public List<G1MiniBlock> GetMiniBlocks(G1MiniBlock g1Mini)
        {
            List<G1MiniBlock> newBlocks = new List<G1MiniBlock>(miniBlocks);
            if (g1Mini == miniBlocks[^1])
                newBlocks.Reverse();
            
            return newBlocks;
        }

        public void OnClickObject()
        {
            _originalPosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            G1GameManager.Instance.hand.transform.position = _originalPosition;
            
            if(currentSate == BlockSate.InMap)
                GetOutOfMap();
        }

        public void OnDragObject()
        {
            if (Vector2.Distance(_originalPosition, playerCamera.ScreenToWorldPoint(Input.mousePosition)) >
                _minDistanceDrag)
            {
                currentSate = BlockSate.IsDragging;
                transform.parent = G1GameManager.Instance.hand;
                G1GameManager.Instance.curG1LevelCtrl.SetupPath();
            }


            if (currentSate == BlockSate.IsDragging)
            {
                var worldPoint = playerCamera.ScreenToWorldPoint(Input.mousePosition);
                G1GameManager.Instance.hand.position = new Vector3(worldPoint.x, worldPoint.y, -1);
                UpdateMiniBlocks();
            }
        }

        public void EndObject()
        {
            switch (currentSate)
            {
                case BlockSate.InBoard:
                    var parent = transform.parent;
                    if (!_isRotating)
                    {
                        _isRotating = true;
                        parent.DORotate(new Vector3(parent.eulerAngles.x, parent.eulerAngles.y, parent.eulerAngles.z + 90),
                            0.25f).SetEase(Ease.Flash).OnComplete(() =>
                        {
                            _isRotating = false;
                        });
                    }
                    break;
                case BlockSate.IsDragging:
                    transform.parent = G1GameManager.Instance.curG1LevelCtrl.transform;
                    if(!CanAddToMap())
                        GetInBoard();
                    else
                        GetInMap();
                    break;
                case BlockSate.InMap:
                    GetInMap();
                    break;
            }
        }
    }
}
