using System.Collections.Generic;
using Game_01.Game_1.Scripts.Gameplay.Block;
using Game_01.Game_1.Scripts.Gameplay.Level;
using Game_01.Game_1.Scripts.UI;
using Game0.Game_01.Game_1.Scripts.Gameplay.Block;
using UnityEngine;

namespace Game0.Game_01.Game_1.Scripts.Gameplay.Level
{
    public class G1LevelCtrl : MonoBehaviour
    {
        public Transform stationStart;
        [SerializeField] private G1ButtonPlay g1ButtonPlay;
        [SerializeField] private Transform originPos;
        [SerializeField] private List<Transform> obstacles = new List<Transform>();
        [SerializeField] private Transform transStart, transEnd;
        [SerializeField] private int width, height;
        [SerializeField] private List<G1MiniBlock> blocksInPath = new List<G1MiniBlock>();
        [SerializeField] private List<G1MiniBlock> blocksInBoard = new List<G1MiniBlock>();
        
        public void SetupLevel()
        {
            G1PathFinding.Instance.CreatePathfinding(width, height, 0.75f, originPos);
            foreach (var obstacle in obstacles)
            {
                G1PathFinding.Instance.GetGrid().GetPathNode(obstacle.position).SetIsWalkable(false);
            }

            G1PathFinding.Instance.GetGrid().GetPathNode(transStart.position).IsStation = true;
            G1PathFinding.Instance.GetGrid().GetPathNode(transEnd.position).IsStation = true;
        }
        
        public void SetupPath()
        {
            SetupPathFinding(transStart);
            SetupPathFinding(transEnd);
        }

        private void SetupPathFinding(Transform trans)
        {
            var node = G1PathFinding.Instance.GetGrid().GetPathNode(trans.position);
            if (node != null && node.G1MiniBlock != null)
            {
                List<G1MiniBlock> path;
                path = G1PathFinding.Instance.FindPathToStation(node.G1MiniBlock);
                for (int i = 0; i < path.Count; i++)
                {
                    if (G1PathFinding.Instance.GetGrid().GetPathNode(path[i].transform.position).IsStation)
                        path[i].SetSpriteCircle();
                    else
                    {
                        if(i+1<path.Count)
                            path[i].SetSpriteByNeighbor(path[i-1].transform.position, path[i+1].transform.position);
                        else if(!G1PathFinding.Instance.GetGrid().GetPathNode(path[i].transform.position).IsStation)
                        {
                            path[i].SetSpriteBase();
                        }
                    }
                }
            }
            
            CheckCanMove();
        }

        public List<G1MiniBlock> GetPathToStation()
        {
            var node = G1PathFinding.Instance.GetGrid().GetPathNode(transStart.position);
            List<G1MiniBlock> path;
            path = G1PathFinding.Instance.FindPathToStation(node.G1MiniBlock);
            return path;
        }

        private void CheckCanMove()
        {
            foreach (var block in blocksInBoard)
            {
                if (block.g1Block.currentSate != BlockSate.InMap)
                    return;
            }
            
            g1ButtonPlay.gameObject.SetActive(true);
            //G1PopupCtrl.Instance.GetPopupByType<G1PopupGameplay>().ShowAccelerator();
        }

        public bool CheckLevelComplete(List<G1MiniBlock> miniBlocks)
        {
            if (!G1PathFinding.Instance.GetGrid().GetPathNode(miniBlocks[0].transform.position).IsStation
                || !G1PathFinding.Instance.GetGrid().GetPathNode(miniBlocks[^1].transform.position).IsStation)
                return false;
                
            bool isComplete = true;
            foreach (var block in blocksInPath)
            {
                if (block.g1Block.currentSate != BlockSate.InMap)
                {
                    block.SetSpriteCanPush(true);
                    isComplete = false;
                }
            }

            return isComplete;
        }

        public void Departure()
        {
            
        }
    }
}
