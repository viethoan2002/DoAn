using UnityEngine;

namespace PathFinding
{
    public class G2_Grid : MonoBehaviour
    {
        private int _width;
        private int _height;
        private float _cellSize;
        private Vector3 _originPosition;
        private G2_PathNode[,] _gridArray;
        [SerializeField] private Transform anchor;
        [SerializeField] private GameObject node;
        
        private void Awake()
        {
            CreateGrid(10,5,1);
        }

        private void CreateGrid(int width, int height, float cellSize)
        {
            this._width = width;
            this._height = height;
            this._cellSize = cellSize;
            this._originPosition = anchor.position;

            _gridArray = new G2_PathNode[width, height];

            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    var newPathNode = Instantiate(node).GetComponent<G2_PathNode>();
                    newPathNode.transform.SetParent(transform);
                    newPathNode.name = $"PathNode ({x}, {y})";
                    newPathNode.transform.position = new Vector2((x + .5f) * cellSize + _originPosition.x,
                        (y + .5f) * cellSize + _originPosition.y);
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
                    _gridArray[x, y].isWalkable = false;
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
            x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize );
            y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
        }

        public void SetPathNode(int x, int y, G2_PathNode value)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                _gridArray[x, y] = value;
            }
        }

        public void SetPathNode(Vector3 worldPosition, G2_PathNode value)
        {
            int x, y;
            GetXY(worldPosition, out x, out y);
            SetPathNode(x, y, value);
        }

        public G2_PathNode GetPathNode(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < _width && y < _height)
            {
                return _gridArray[x, y];
            }
            else
            {
                return default(G2_PathNode);
            }
        }

        public G2_PathNode GetPathNode(Vector3 worldPosition)
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
                    _gridArray[x, y].SetPreview(false, new Color(0, 0, 0, 0));
                }
            }
        }
    }
}
