using System;
using Card;
using Raycast;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PathFinding
{
    public class G2_PathNode : MonoBehaviour,IClickHandle
    {
        private G2_Grid _g2Grid;
        [FormerlySerializedAs("X")] public int x;
        [FormerlySerializedAs("Y")] public int y;

        [FormerlySerializedAs("GCost")] public int gCost;
        [FormerlySerializedAs("HCost")] public int hCost;
        [FormerlySerializedAs("FCost")] public int fCost;

        [FormerlySerializedAs("IsWalkable")] public bool isWalkable;
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        [FormerlySerializedAs("IsPreview")] public bool isPreview;

        [FormerlySerializedAs("CameFromNode")] public G2_PathNode cameFromNode;

        public G2_BaseCard cardInNode;
        public static Action<G2_PathNode> SelectPathNode;

        public void SetPathNode(G2_Grid g2Grid, int x, int y)
        {
            this._g2Grid = g2Grid;
            this.x = x;
            this.y = y;
            isWalkable = true;
        }

        public void CalculateFCost() {
            fCost = gCost + hCost;
        }

        public void SetIsWalkable(bool en) {
            this.isWalkable = en;
        }

        public void SetPreview(bool en,Color color)
        {
            isPreview = en;
            spriteRenderer.color = color;
            spriteRenderer.enabled = en;
        }
        
        public override string ToString() {
            return x + "," + y;
        }

        private void OnMouseDown()
        {
            Debug.Log(x + " " + y);
        }

        public void OnClickObject()
        {
            SelectPathNode?.Invoke(this);
        }

        public void OnDragObject()
        {
            
        }

        public void EndObject()
        {
            
        }
    }
}
