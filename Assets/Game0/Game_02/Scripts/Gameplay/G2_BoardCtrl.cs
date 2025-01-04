using System.Collections.Generic;
using System.IO;
using _Camera;
using Card;
using DG.Tweening;
using Game0.Game_02.Scripts.UI;
using PathFinding;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class G2_BoardCtrl : MonoBehaviour
    {
        public static G2_BoardCtrl Instance;
        [FormerlySerializedAs("pathfinding")] [SerializeField] private G2_Pathfinding g2Pathfinding;

        [SerializeField] private List<G2_WayCard> wayCardsInBoard = new List<G2_WayCard>();
        [SerializeField] private List<G2_EnemyCard> enemyCardsInBoard = new List<G2_EnemyCard>();

        [FormerlySerializedAs("wayCardOrigin")] [SerializeField] private G2_WayCard g2WayCardOrigin;
        [SerializeField] private List<G2_TreasureCard> treasureCardsInBoard = new List<G2_TreasureCard>();
        
        [SerializeField] private List<G2_WayCard> obstacleCardsInBoard = new List<G2_WayCard>();

        [SerializeField] private List<G2_PathNode> nodesCanThrow = new List<G2_PathNode>();
        [SerializeField] private List<G2_PathNode> _nodesBaseMap = new List<G2_PathNode>();

        [SerializeField] private G2_BaseCard selectedCard;
        private int _indexWaitSpawnEnemy;

        private void Awake()
        {
            Instance = this;
        }
        
        private void OnEnable()
        {
            G2_PathNode.SelectPathNode += ThrowCardToBoard;
        }

        private void OnDisable()
        {
            G2_PathNode.SelectPathNode -= ThrowCardToBoard;
        }

        public void SetupDefaultBoard()
        {
            ClearMap();
            
            _nodesBaseMap.Add(g2Pathfinding.GetGrid().GetPathNode(g2WayCardOrigin.transform.position));
            AddWayCardToBoard(g2WayCardOrigin);

            foreach (var card in treasureCardsInBoard)
            {
                card.SetupTreasure(false);
                var node = g2Pathfinding.GetGrid().GetPathNode(card.transform.position);
                node.isWalkable = false;
                node.cardInNode = card;
                _nodesBaseMap.Add(node);
            }

            obstacleCardsInBoard.Clear();
            for (int i = 0; i < 4; i++)
            {
                G2_PathNode node = GetPathNodeEmpty();
                var obstacle = G2_ObjectPool.Instance.Get((G2_ObjectPool.Instance.obstacle)).GetComponent<G2_BaseCard>();
                obstacle.transform.position=node.transform.position;
                obstacleCardsInBoard.Add(obstacle.GetComponent<G2_WayCard>());
                
                node.cardInNode = obstacle;
                node.isWalkable = false;
            }

            treasureCardsInBoard[UnityEngine.Random.Range(0, treasureCardsInBoard.Count)].SetupTreasure(true);
            _indexWaitSpawnEnemy = 6;
        }

        private void ClearMap()
        {
            g2Pathfinding.GetGrid().DeActiveAllNode();
            for (int i = 0; i < g2Pathfinding.GetGrid().GetWidth(); i++)
            {
                for (int j = 0; j < g2Pathfinding.GetGrid().GetHeight(); j++)
                {
                    G2_PathNode node = g2Pathfinding.GetGrid().GetPathNode(i, j);
                    if (!_nodesBaseMap.Contains(node) && node.cardInNode != null)
                    {
                        var interactCard = node.cardInNode.GetComponent<G2_InteractCardBtn>();
                        if (interactCard != null)
                        {
                            G2_SpawnCard.Instance.ReturnInteractCard(interactCard);
                        }
                        else
                        {
                            G2_ObjectPool.Instance.Return(node.cardInNode.gameObject, true);
                        }
                        
                        node.cardInNode = null;
                        node.isWalkable = true;
                    }
                }
            }
            
            _nodesBaseMap.Clear();
            wayCardsInBoard.Clear();
        }

        #region Display Board

        public void ShowNodeCanThrowWayCard(G2_WayCard g2WayCard)
        {
            HideNodeCanThrow();

            int x, y;
            Vector2Int dir;
            G2_PathNode node;
            foreach (var way in wayCardsInBoard)
            {
                g2Pathfinding.GetGrid().GetXY(way.transform.position, out x, out y);
                foreach (var direction in way.directionsCanMove)
                {
                    dir = way.GetVector2ByDirectionType(direction);
                    node = g2Pathfinding.GetGrid().GetPathNode(x + dir.x, y + dir.y);
                    if (node != null && node.isWalkable && g2WayCard.IsOpen(-dir) && !_nodesBaseMap.Contains(node))
                    {
                        node.SetPreview(true, new Color(1, 1, 0, 0.35f));
                        if (!nodesCanThrow.Contains(node))
                            nodesCanThrow.Add(node);
                    }
                }
            }
        }

        public void ShowNodeCanThrowBombCard()
        {
            G2_PathNode node;

            foreach (var car in wayCardsInBoard)
            {
                node = g2Pathfinding.GetGrid().GetPathNode(car.transform.position);
                if (!node.isWalkable && !_nodesBaseMap.Contains(node))
                {
                    node.SetPreview(true, new Color(1, 0, 0, 0.5f));
                    if (!nodesCanThrow.Contains(node))
                        nodesCanThrow.Add(node);
                }
            }

            foreach (var car in obstacleCardsInBoard)
            {
                node = g2Pathfinding.GetGrid().GetPathNode(car.transform.position);
                node.SetPreview(true, new Color(1, 0, 0, 0.5f));
                if (!nodesCanThrow.Contains(node))
                    nodesCanThrow.Add(node);
            }
        }

        public void ShowNodeTreasure()
        {
            G2_PathNode node;
            foreach (var t in treasureCardsInBoard)
            {
                if (!t.IsOpen())
                {
                    node = g2Pathfinding.GetGrid().GetPathNode(t.transform.position);
                    node.SetPreview(true, new Color(0, 1, 0, 0.35f));
                    if (!nodesCanThrow.Contains(node))
                        nodesCanThrow.Add(node);
                }   
            }
        }

        public void ShowNodeHasEnemy()
        {
            ShowNodeCanThrowByList(enemyCardsInBoard, new Color(1, 0, 0, 0.5f));
        }

        private void ShowNodeCanThrowByList<T>(List<T> list, Color color) where T : G2_BaseCard
        {
            G2_PathNode node;
            foreach (var t in list)
            {
                node = g2Pathfinding.GetGrid().GetPathNode(t.transform.position);
                node.SetPreview(true, color);
                if (!nodesCanThrow.Contains(node))
                    nodesCanThrow.Add(node);
            }
        }

        public void HideNodeCanThrow()
        {
            g2Pathfinding.GetGrid().DeActiveAllNode();
            nodesCanThrow.Clear();
        }

        #endregion


        #region Interact Board

        public void SetSelectedCard(G2_BaseCard card)
        {
            this.selectedCard = card;
        }

        public G2_BaseCard GetSelectedCard()
        {
            return selectedCard;
        }

        private void ThrowCardToBoard(G2_PathNode node)
        {
            if (selectedCard == null || selectedCard.GetComponent<ICardInteract>().IsInteractable())
                return;
            
            if (selectedCard is G2_WayCard)
                ThrowWayCardToBoard(node);

            if (selectedCard is G2_BombCard)
                ThrowBombCardToBoard(node);

            if (selectedCard is G2_CompassCard)
                ThrowCompassCardToBoard(node);
            if (selectedCard is G2_AttackCard)
                ThrowAttackCardToBoard(node);
        }

        private void ThrowWayCardToBoard(G2_PathNode node)
        {
            if (nodesCanThrow.Contains(node))
            {
                G2_PopupCtrl.Instance.ActiveButtonRevert(false);
                Vector3 pos = new Vector3(node.transform.position.x, node.transform.position.y, -1);
                selectedCard.GetComponent<G2_WayCard>()
                    .MoveToBoard(pos, CheckFindIsland);

                node.cardInNode = selectedCard;
                node.isWalkable = false;

                wayCardsInBoard.Add((G2_WayCard)selectedCard);
                selectedCard = null;

                HideNodeCanThrow();
                CameraManager.Instance.CameraZoomIn();
                InteractCardManager.Instance.ShowSpellCard();

                _indexWaitSpawnEnemy -= 1;
            }
        }

        private void ThrowBombCardToBoard(G2_PathNode node)
        {
            if (nodesCanThrow.Contains(node))
            {
                G2_PopupCtrl.Instance.ActiveButtonRevert(false);
                selectedCard.GetComponent<G2_BombCard>().MoveToBoard(node);

                if (node.cardInNode is G2_WayCard)
                {
                    wayCardsInBoard.Remove((G2_WayCard)node.cardInNode);
                    obstacleCardsInBoard.Remove((G2_WayCard)node.cardInNode);
                }

                node.isWalkable = true;
                selectedCard = null;

                HideNodeCanThrow();
                CameraManager.Instance.CameraZoomIn();
                InteractCardManager.Instance.ShowSpellCard();

                _indexWaitSpawnEnemy -= 1;
            }
        }

        private void ThrowAttackCardToBoard(G2_PathNode node)
        {
            if (nodesCanThrow.Contains(node))
            {
                G2_PopupCtrl.Instance.ActiveButtonRevert(false);
                selectedCard.GetComponent<G2_AttackCard>().MoveToBoard(node);

                if (node.cardInNode is G2_EnemyCard)
                    enemyCardsInBoard.Remove((G2_EnemyCard)node.cardInNode);

                node.isWalkable = true;
                selectedCard = null;

                HideNodeCanThrow();
                CameraManager.Instance.CameraZoomIn();
                InteractCardManager.Instance.ShowSpellCard();

                _indexWaitSpawnEnemy -= 1;
            }
        }

        private void ThrowCompassCardToBoard(G2_PathNode node)
        {
            if (nodesCanThrow.Contains(node))
            {
                G2_PopupCtrl.Instance.ActiveButtonRevert(false);
                G2_SpawnCard.Instance.ReturnInteractCard(selectedCard.GetComponent<G2_InteractCardBtn>());
                node.cardInNode.GetComponent<G2_TreasureCard>().SuggestTreasure(() =>
                {
                    CameraManager.Instance.CameraZoomIn();
                    InteractCardManager.Instance.ShowSpellCard();
                });
                HideNodeCanThrow();

                selectedCard = null;
                _indexWaitSpawnEnemy -= 1;
            }
        }

        public void ThrowEnemy()
        {
            if (_indexWaitSpawnEnemy == 0)
                ThrowEnemyCardToBoard();
        }

        private void ThrowEnemyCardToBoard()
        {
            if(wayCardsInBoard.Count == 1)
                return;
            
            var enemy = G2_ObjectPool.Instance.Get(G2_ObjectPool.Instance.enemy).GetComponent<G2_EnemyCard>();
            var nodeSpawn = g2Pathfinding.GetGrid()
                .GetPathNode(wayCardsInBoard[UnityEngine.Random.Range(1, wayCardsInBoard.Count)].transform.position);
            var effect = G2_ObjectPool.Instance.Get(G2_ObjectPool.Instance.fire);
            effect.transform.position = nodeSpawn.transform.position;
            enemy.transform.parent = transform;
            enemy.transform.position = nodeSpawn.transform.position;
            if (nodeSpawn.cardInNode != null)
            {
                wayCardsInBoard.Remove((G2_WayCard)nodeSpawn.cardInNode);
                G2_SpawnCard.Instance.ReturnInteractCard(nodeSpawn.cardInNode.GetComponent<G2_InteractCardBtn>());
            }

            nodeSpawn.cardInNode = enemy;
            nodeSpawn.isWalkable = false;
            enemyCardsInBoard.Add(enemy);
            _indexWaitSpawnEnemy = 6;
        }

        #endregion

        public G2_PathNode GetPathNodeEmpty()
        {
            G2_PathNode node = null;
            int x, y;
            while (node==null)
            {
                x = UnityEngine.Random.Range(1, g2Pathfinding.GetGrid().GetWidth() - 1);
                y = UnityEngine.Random.Range(0, g2Pathfinding.GetGrid().GetHeight());
                if(g2Pathfinding.GetGrid().GetPathNode(x,y).cardInNode==null)
                    node = g2Pathfinding.GetGrid().GetPathNode(x,y);
            }

            return node;
        }

        public void AddWayCardToBoard(G2_WayCard g2WayCard)
        {
            wayCardsInBoard.Add(g2WayCard);
            g2Pathfinding.GetGrid().GetPathNode(g2WayCard.transform.position).isWalkable = false;
            g2Pathfinding.GetGrid().GetPathNode(g2WayCard.transform.position).cardInNode = g2WayCard;
        }

        private void CheckFindIsland()
        {
            foreach (var treasure in treasureCardsInBoard)
            {
                CheckIsland(treasure, Vector2Int.up);
                CheckIsland(treasure, Vector2Int.right);
                CheckIsland(treasure, Vector2Int.down);
                CheckIsland(treasure, Vector2Int.left);
            }
        }

        private void CheckIsland(G2_TreasureCard g2TreasureCard, Vector2Int direction)
        {
            int x, y;
            g2Pathfinding.GetGrid().GetXY(g2TreasureCard.transform.position, out x, out y);
            G2_PathNode node = g2Pathfinding.GetGrid().GetPathNode(x + direction.x, y + direction.y);
            if (node != null && node.cardInNode != null)
            {
                G2_WayCard g2WayCard = node.cardInNode.GetComponent<G2_WayCard>();
                if (g2WayCard != null && g2WayCard.IsOpen(-direction) && CheckPathToTreasure(g2WayCard))
                    g2TreasureCard.ShowTreasure();
            }
        }

        private bool CheckPathToTreasure(G2_WayCard g2WayCard)
        {
            List<G2_PathNode> path = g2Pathfinding.FindPath(wayCardsInBoard[0].transform.position,
                g2WayCard.transform.position);
            if (path == null)
                return false;
            else
                return true;
            //return path != null;
        }
    }
}
