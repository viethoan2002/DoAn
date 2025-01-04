 using System;
 using System.Collections.Generic;
 using Card;
 using UnityEngine;
using UnityEngine.Serialization;

namespace PathFinding
{
    [Serializable]
    public struct Obstacle
    {
        [FormerlySerializedAs("X")] public int x;
        [FormerlySerializedAs("Y")] public int y;
        [FormerlySerializedAs("Width")] public int width;
        [FormerlySerializedAs("Height")] public int height;
    }
    public class G2_Pathfinding : MonoBehaviour
    {
        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 14;

        [FormerlySerializedAs("grid")] [SerializeField] private G2_Grid g2Grid;
        [SerializeField] private List<Obstacle> obstacles = new List<Obstacle>();
        private List<G2_PathNode> _openList;
        private List<G2_PathNode> _closedList;

        public void CreatePathfinding(int width, int height,float cellSize)
        {
            foreach (var obstacle in obstacles)
            {
                for (int i = 0; i < obstacle.width; i++)
                {
                    for (int j = 0; j < obstacle.height; j++)
                    {
                        g2Grid.GetPathNode(obstacle.x+i,obstacle.y+j).SetIsWalkable(false);
                    }
                }
            }
        }
        
        public G2_Grid GetGrid()
        {
            return g2Grid;
        }

        public List<G2_PathNode> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
            g2Grid.GetXY(startWorldPosition, out int startX, out int startY);
            g2Grid.GetXY(endWorldPosition, out int endX, out int endY);

            return FindPath(startX, startY, endX, endY);
        }

        public List<G2_PathNode> FindPath(int startX, int startY, int endX, int endY) {
            G2_PathNode startNode = g2Grid.GetPathNode(startX, startY);
            G2_PathNode endNode = g2Grid.GetPathNode(endX, endY);

            if (startNode == null || endNode == null) {
                // Invalid Path
                return null;
            }

            _openList = new List<G2_PathNode> { startNode };
            _closedList = new List<G2_PathNode>();

            for (int x = 0; x < g2Grid.GetWidth(); x++) {
                for (int y = 0; y < g2Grid.GetHeight(); y++) {
                    G2_PathNode g2PathNode = g2Grid.GetPathNode(x, y);
                    g2PathNode.gCost = 99999999;
                    g2PathNode.CalculateFCost();
                    g2PathNode.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (_openList.Count > 0) {
                G2_PathNode currentNode = GetLowestFCostNode(_openList);
                if (currentNode == endNode) {
                    return CalculatePath(endNode);
                }

                _openList.Remove(currentNode);
                _closedList.Add(currentNode);

                foreach (G2_PathNode neighbourNode in GetNeighbourList(currentNode)) {
                    if (!CheckNodeCanMove(currentNode, neighbourNode)) {
                        continue;
                    }

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost) {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (!_openList.Contains(neighbourNode)) {
                            _openList.Add(neighbourNode);
                        }
                    }
                }
            }

            // Out of nodes on the openList
            return null;
        }

        private List<G2_PathNode> GetNeighbourList(G2_PathNode currentNode) {
            List<G2_PathNode> neighbourList = new List<G2_PathNode>();

            if (currentNode.x - 1 >= 0 ) {
                // Left
                if(GetNode(currentNode.x - 1, currentNode.y).cardInNode!=null && GetNode(currentNode.x - 1, currentNode.y).cardInNode is G2_WayCard)
                    neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            }
            if (currentNode.x + 1 < g2Grid.GetWidth()) {
                // Right
                if(GetNode(currentNode.x + 1, currentNode.y).cardInNode!=null && GetNode(currentNode.x + 1, currentNode.y).cardInNode is G2_WayCard)
                    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            }
            // Down
            if (currentNode.y - 1 >= 0)
            {
                if(GetNode(currentNode.x, currentNode.y - 1).cardInNode!= null && GetNode(currentNode.x, currentNode.y - 1).cardInNode is G2_WayCard) 
                    neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
            }
            // Up
            if (currentNode.y + 1 < g2Grid.GetHeight())
            {
                if(GetNode(currentNode.x, currentNode.y + 1).cardInNode!= null && GetNode(currentNode.x, currentNode.y + 1).cardInNode is G2_WayCard) 
                    neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));
            }

            return neighbourList;
        }

        public G2_PathNode GetNode(int x, int y) {
            return g2Grid.GetPathNode(x, y);
        }

        private List<G2_PathNode> CalculatePath(G2_PathNode endNode) {
            List<G2_PathNode> path = new List<G2_PathNode>();
            path.Add(endNode);
            G2_PathNode currentNode = endNode;
            while (currentNode.cameFromNode != null) {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        public bool CheckNodeCanMove(G2_PathNode currentNode, G2_PathNode endNode)
        {
            if (endNode.cardInNode== null) 
                return false;
            
            var currentWayCard = (G2_WayCard)currentNode.cardInNode;
            var endWayCards = (G2_WayCard)endNode.cardInNode;
            if (currentWayCard==null || endWayCards==null)
                return false;
            
            if(endWayCards.isDeadEnd)
                return false;

            Vector2Int direction = new Vector2Int(endNode.x - currentNode.x, endNode.y - currentNode.y);
            if (currentWayCard.IsOpen(direction) && endWayCards.IsOpen(-direction))
                return true;
            else
                return false;
        }

        private int CalculateDistanceCost(G2_PathNode a, G2_PathNode b) {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * remaining;
        }

        private G2_PathNode GetLowestFCostNode(List<G2_PathNode> pathNodeList) {
            G2_PathNode lowestFCostNode = pathNodeList[0];
            for (int i = 1; i < pathNodeList.Count; i++) {
                if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                    lowestFCostNode = pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }
    }
}
