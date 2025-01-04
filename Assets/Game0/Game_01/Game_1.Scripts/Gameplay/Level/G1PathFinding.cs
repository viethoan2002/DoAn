using System;
using System.Collections.Generic;
using Game_01.Game_1.Scripts.Gameplay.Block;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game_01.Game_1.Scripts.Gameplay.Level
{
    [Serializable]
    public struct Obstacle
    {
        [FormerlySerializedAs("X")] public int x;
        [FormerlySerializedAs("Y")] public int y;
        [FormerlySerializedAs("Width")] public int width;
        [FormerlySerializedAs("Height")] public int height;
    }
    public class G1PathFinding : MonoBehaviour
    {
        public static G1PathFinding Instance;
        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 14;

        [FormerlySerializedAs("grid")] [SerializeField] private G1Grid g1Grid;
        //[SerializeField] private List<Obstacle> obstacles = new List<Obstacle>();
        private List<G1PathNode> _openList;
        private List<G1PathNode> _closedList;

        private void Awake()
        {
            Instance = this;
        }
        
        public void CreatePathfinding(int width, int height,float cellSize ,Transform originPosition)
        {
            g1Grid.CreateGrid(width, height, cellSize, originPosition.position);
            /*foreach (var obstacle in obstacles)
            {
                for (int i = 0; i < obstacle.width; i++)
                {
                    for (int j = 0; j < obstacle.height; j++)
                    {
                        grid.GetPathNode(obstacle.x+i,obstacle.y+j).SetIsWalkable(false);
                    }
                }
            }*/
        }


        public G1Grid GetGrid()
        {
            return g1Grid;
        }

        public List<G1PathNode> FindPath(Vector3 positionPlayer, int distance)
        {
            g1Grid.GetXY(positionPlayer, out int startX, out int startY);
            List<G1PathNode> vectorPath = new List<G1PathNode>();
            G1PathNode playerNode = g1Grid.GetPathNode(startX, startY);
          

            for (int i = 0; i <= distance; i++)
            {
                var node = g1Grid.GetPathNode(startX - i, startY);
                if (node == null)
                    break;
                else
                {
                    if (node.IsWalkable && !vectorPath.Contains(node))
                        vectorPath.Add(node);
                    
                    for (int j = 0; j <= distance - i; j++)
                    {
                        var nNode = g1Grid.GetPathNode(startX - i, startY - j);
                        if (nNode == null)
                            break;
                        else if (nNode.IsWalkable && !vectorPath.Contains(nNode))
                            vectorPath.Add(nNode);
                    }

                    for (int j = 1; j <= distance - i; j++)
                    {
                        var nNode = g1Grid.GetPathNode(startX - i, startY + j);
                        if (nNode == null)
                            break;
                        else if (nNode.IsWalkable && !vectorPath.Contains(nNode))
                            vectorPath.Add(nNode);
                    }
                }
            }

            for (int i = 1; i <= distance; i++)
            {
                var node = g1Grid.GetPathNode(startX + i, startY);
                if (node == null)
                    break;
                else
                {
                    if (node.IsWalkable && !vectorPath.Contains(node))
                        vectorPath.Add(node);

                    for (int j = 0; j <= distance - i; j++)
                    {
                        var nNode = g1Grid.GetPathNode(startX + i, startY - j);
                        if (nNode == null)
                            break;
                        else if (nNode.IsWalkable && !vectorPath.Contains(nNode))
                            vectorPath.Add(nNode);
                    }

                    for (int j = 1; j <= distance - i; j++)
                    {
                        var nNode = g1Grid.GetPathNode(startX + i, startY + j);
                        if (nNode == null)
                            break;
                        else if (nNode.IsWalkable && !vectorPath.Contains(nNode))
                            vectorPath.Add(nNode);
                    }
                }
            }

            for (int i = 0; i <= distance; i++)
            {
                var node = g1Grid.GetPathNode(startX, startY - i);
                if (node == null)
                    break;
                else
                {
                    if (node.IsWalkable && !vectorPath.Contains(node))
                        vectorPath.Add(node);

                    for (int j = 0; j <= distance - i; j++)
                    {
                        var nNode = g1Grid.GetPathNode(startX - j, startY - i);
                        if (nNode == null)
                            break;
                        else if (nNode.IsWalkable && !vectorPath.Contains(nNode))
                            vectorPath.Add(nNode);

                    }

                    for (int j = 1; j <= distance - i; j++)
                    {
                        var nNode = g1Grid.GetPathNode(startX + j, startY - i);
                        if (nNode == null)
                            break;
                        else if (nNode.IsWalkable && !vectorPath.Contains(nNode))
                            vectorPath.Add(nNode);

                    }
                }
            }

            for (int i = 1; i <= distance; i++)
            {
                var node = g1Grid.GetPathNode(startX, startY + i);
                if (node == null)
                    break;
                else
                {
                    if (node.IsWalkable && !vectorPath.Contains(node))
                        vectorPath.Add(node);

                    for (int j = 0; j <= distance - i; j++)
                    {
                        var nNode = g1Grid.GetPathNode(startX - j, startY + i);
                        if (nNode == null)
                            break;
                        else if (nNode.IsWalkable && !vectorPath.Contains(nNode))
                            vectorPath.Add(nNode);

                    }

                    for (int j = 1; j <= distance - i; j++)
                    {
                        var nNode = g1Grid.GetPathNode(startX + j, startY + i);
                        if (nNode == null)
                            break;
                        else if (nNode.IsWalkable && !vectorPath.Contains(nNode))
                            vectorPath.Add(nNode);

                    }
                }
            }

            return vectorPath;
        }

        public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
            g1Grid.GetXY(startWorldPosition, out int startX, out int startY);
            g1Grid.GetXY(endWorldPosition, out int endX, out int endY);

            List<G1PathNode> path = FindPath(startX, startY, endX, endY);
            if (path == null) {
                return null;
            } else {
                List<Vector3> vectorPath = new List<Vector3>();
                foreach (G1PathNode pathNode in path) {
                    vectorPath.Add(new Vector3(pathNode.X, pathNode.Y) * g1Grid.GetCellSize() + Vector3.one * g1Grid.GetCellSize() * .5f + g1Grid.GetOriginPosition());
                }
                return vectorPath;
            }
        }

        public List<G1PathNode> FindPath(int startX, int startY, int endX, int endY) {
            G1PathNode startNode = g1Grid.GetPathNode(startX, startY);
            G1PathNode endNode = g1Grid.GetPathNode(endX, endY);

            if (startNode == null || endNode == null) {
                // Invalid Path
                return null;
            }

            _openList = new List<G1PathNode> { startNode };
            _closedList = new List<G1PathNode>();

            for (int x = 0; x < g1Grid.GetWidth(); x++) {
                for (int y = 0; y < g1Grid.GetHeight(); y++) {
                    G1PathNode g1PathNode = g1Grid.GetPathNode(x, y);
                    g1PathNode.GCost = 99999999;
                    g1PathNode.CalculateFCost();
                    g1PathNode.CameFromNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (_openList.Count > 0) {
                G1PathNode currentNode = GetLowestFCostNode(_openList);
                if (currentNode == endNode) {
                    return CalculatePath(endNode);
                }

                _openList.Remove(currentNode);
                _closedList.Add(currentNode);

                foreach (G1PathNode neighbourNode in GetNeighbourList(currentNode)) {
                    if (_closedList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.IsWalkable) {
                        _closedList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.GCost) {
                        neighbourNode.CameFromNode = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateDistanceCost(neighbourNode, endNode);
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

        private List<G1PathNode> GetNeighbourList(G1PathNode currentNode) {
            List<G1PathNode> neighbourList = new List<G1PathNode>();

            if (currentNode.X - 1 >= 0) {
                // Left
                neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y));
                // Left Down
                if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
                // Left Up
                if (currentNode.Y + 1 < g1Grid.GetHeight()) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
            }
            if (currentNode.X + 1 < g1Grid.GetWidth()) {
                // Right
                neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y));
                // Right Down
                if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));
                // Right Up
                if (currentNode.Y + 1 < g1Grid.GetHeight()) neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
            }
            // Down
            if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X, currentNode.Y - 1));
            // Up
            if (currentNode.Y + 1 < g1Grid.GetHeight()) neighbourList.Add(GetNode(currentNode.X, currentNode.Y + 1));

            return neighbourList;
        }

        public G1PathNode GetNode(int x, int y) {
            return g1Grid.GetPathNode(x, y);
        }

        private List<G1PathNode> CalculatePath(G1PathNode endNode) {
            List<G1PathNode> path = new List<G1PathNode>();
            path.Add(endNode);
            G1PathNode currentNode = endNode;
            while (currentNode.CameFromNode != null) {
                path.Add(currentNode.CameFromNode);
                currentNode = currentNode.CameFromNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(G1PathNode a, G1PathNode b) {
            int xDistance = Mathf.Abs(a.X - b.X);
            int yDistance = Mathf.Abs(a.Y - b.Y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * remaining;
        }

        private G1PathNode GetLowestFCostNode(List<G1PathNode> pathNodeList) {
            G1PathNode lowestFCostNode = pathNodeList[0];
            for (int i = 1; i < pathNodeList.Count; i++) {
                if (pathNodeList[i].FCost < lowestFCostNode.FCost) {
                    lowestFCostNode = pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }

        public List<G1MiniBlock> FindPathToStation(G1MiniBlock g1MiniBlock)
        {
            List<G1MiniBlock> miniPath = new List<G1MiniBlock>();
            FindPathToStationRecursive(g1MiniBlock, miniPath, new List<G1MiniBlock>());
            return miniPath;
        }

        private void FindPathToStationRecursive(G1MiniBlock g1MiniBlock,List<G1MiniBlock> miniPath,List<G1MiniBlock> miniPathClone)
        {
            if (miniPathClone.Contains(g1MiniBlock))
                return;
            else
            {
                miniPathClone.AddRange(g1MiniBlock.g1Block.GetMiniBlocks(g1MiniBlock));
                if (miniPath.Count < miniPathClone.Count)
                {
                    miniPath.Clear();
                    miniPath.AddRange(miniPathClone);
                }
                else if(miniPath.Count == miniPathClone.Count)
                {
                    if (g1Grid.GetPathNode(miniPathClone[^1].transform.position).IsStation)
                    {
                        miniPath.Clear();
                        miniPath.AddRange(miniPathClone);
                    }
                }
            }
            
            if (g1Grid.GetPathNode(miniPathClone[^1].transform.position).IsStation)
            {
                return;
            }
            
            int x=0, y=0;
            g1Grid.GetXY(miniPathClone[^1].transform.position, out x, out y);
            var node = g1Grid.GetPathNode(x + 1, y);
            
            if (CheckMiniNodeCanMove(node,miniPathClone))
                FindPathToStationRecursive(node.G1MiniBlock,miniPath,new List<G1MiniBlock>(miniPathClone));
            
            node = g1Grid.GetPathNode(x - 1, y);
            if (CheckMiniNodeCanMove(node,miniPathClone))
                FindPathToStationRecursive(node.G1MiniBlock,miniPath,new List<G1MiniBlock>(miniPathClone));

            node = g1Grid.GetPathNode(x, y - 1);
            if (CheckMiniNodeCanMove(node,miniPathClone))
                FindPathToStationRecursive(node.G1MiniBlock,miniPath,new List<G1MiniBlock>(miniPathClone));

            node = g1Grid.GetPathNode(x, y + 1);
            if (CheckMiniNodeCanMove(node,miniPathClone))
                FindPathToStationRecursive(node.G1MiniBlock,miniPath,new List<G1MiniBlock>(miniPathClone));
        }

        private bool CheckMiniNodeCanMove(G1PathNode node, List<G1MiniBlock> miniPath)
        {
            if (node != null && node.G1MiniBlock != null && !miniPath.Contains(node.G1MiniBlock) && node.G1MiniBlock.isHead)
                return true;
            else
                return false;
        }
    }
}
