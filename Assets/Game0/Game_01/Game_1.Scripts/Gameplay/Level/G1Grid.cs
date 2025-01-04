using UnityEngine;

namespace Game_01.Game_1.Scripts.Gameplay.Level
{
    public class G1Grid : MonoBehaviour
    {
        private int _width;
        private int _height;
        private float _cellSize;
        private Vector3 _originPosition;
        private G1PathNode[,] _gridArray;
    
        public void CreateGrid(int width, int height, float cellSize, Vector3 originPosition)
        {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;
            this._originPosition = originPosition;

            _gridArray = new G1PathNode[width, height];

            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    //var newPathNode = ObjectPool.Instance.Get(ObjectPool.Instance.node).GetComponent<PathNode>();
                    var newPathNode = new G1PathNode();
                    /*newPathNode.transform.position = new Vector2((x + .5f) * cellSize + originPosition.x,
                        (y + .5f) * cellSize + originPosition.y);*/
                    newPathNode.SetPathNode(this, x, y);
                    _gridArray[x, y] = newPathNode;
                }
            }
        }

        public void ResetGrid()
        {
            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    _gridArray[x, y].IsWalkable = true;
                }
            }
        }

        public int GetWidth()
        {
            return _width;
        }

        public int GetHeight()
        {
            return _height;
        }

        public float GetCellSize()
        {
            return _cellSize;
        }

        public Vector3 GetOriginPosition()
        {
            return _originPosition;
        }

        public bool ObjInGrid(int x,int y)
        {
            if (x < 0 || x > _width || y < 0 || y > _height)
                return false;
            else
                return true;
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * _cellSize + _originPosition;
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(((worldPosition - _originPosition).x + _cellSize / 2) / _cellSize);
            y = Mathf.FloorToInt(((worldPosition - _originPosition).y + _cellSize / 2) / _cellSize);
        }

        public void SetPathNode(int x, int y, G1PathNode value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = value;
            }
        }

        public void SetPathNode(Vector3 worldPosition, G1PathNode value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetPathNode(x, y, value);
        }

        public G1PathNode GetPathNode(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _gridArray[x, y];
            }
            else
            {
                return null;
            }
        }

        public G1PathNode GetPathNode(Vector3 worldPosition)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetPathNode(x, y);
        }

        public void DeActiveAllNode()
        {
            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                }
            }
        }
    }
}
