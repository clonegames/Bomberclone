using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberclone {

    public class MapManager : MonoBehaviour {

        public int BoardSize;

        [SerializeField]
        private GameObject m_prefabHardWall, m_prefabSoftWall, m_prefabBomb;

        private Vector3 m_bounds;

        private List<Cell> m_grid;
        
        private void Start() {
            Generate();            
        }     

        [ContextMenu("Generate")]
        public void Generate() {

            if (BoardSize % 2 != 1) {
                BoardSize++;
            }

            var collider = GetComponent<BoxCollider>();
            m_bounds = new Vector3(collider.bounds.extents.x, 0, collider.bounds.extents.z);
            var offset = m_bounds - (m_bounds / BoardSize);

            if (m_grid != null) {
                Clear();
            }

            m_grid = new List<Cell>(BoardSize * BoardSize);

            for (int x = 0; x < BoardSize; x++) {
                for (int y = 0; y < BoardSize; y++) {

                    var position = new Vector3(m_bounds.x * ((float)x / BoardSize) * 2, 0, m_bounds.z * ((float)y / BoardSize) * 2) - offset;
                    if ((x % 2  == 0 && y % 2 == 0) || x == 0 || y == 0 || x == BoardSize - 1 || y == BoardSize - 1) {
                        m_grid.Add(new Cell(m_prefabHardWall, position, x + (y * BoardSize), Cell.CellType.HardWall, transform));
                        continue;
                    }

                    var rand = Random.Range(0, 100);
                    if (rand < 70) {
                        m_grid.Add(new Cell(null, position, x + (y * BoardSize), Cell.CellType.Empty, transform));      
                    }
                    else {
                        m_grid.Add(new Cell(m_prefabSoftWall, position, x + (y * BoardSize), Cell.CellType.SoftWall, transform));
                    }                     
                }
            }
        }

        [ContextMenu("Clear")]
        private void Clear() {

            var childCount = transform.childCount;
            Debug.Log("Filhos:" + childCount);
            for (int i = childCount - 1; i >= 0; i--) {
#if UNITY_EDITOR
                if (Application.isEditor){
                    DestroyImmediate(transform.GetChild(i).gameObject);
                    continue;
                }
#endif
                Debug.Log("Destrui");
                Destroy(transform.GetChild(i).gameObject);
            }

            if (m_grid != null) {
                m_grid.Clear();
                m_grid = null;
            }               
        }

        public Cell GetNeighbourSquare(int id, Direction direction) {

            int squareId = -1;

            switch (direction) {
                case Direction.up:
                    if (id > BoardSize * BoardSize - (BoardSize + 1)) {
                        return null;
                    }
                    squareId = id + BoardSize;
                    break;
                case Direction.right:
                    if (id % BoardSize == BoardSize - 1) {
                        return null;
                    }
                    squareId = id + 1;
                    break;
                case Direction.down:
                    if (id < BoardSize) {
                        return null;
                    }
                    squareId = id - BoardSize;
                    break;
                case Direction.left:
                    if (id % BoardSize == 0) {
                        return null;
                    }
                    squareId = id - 1;
                    break;              
            }
            if (squareId == -1) {
                Debug.LogWarning("Something went wrong when searching the cell at direction " + direction + "!");
                return null;
            }

            return m_grid[squareId];
        }
    }

    public enum Direction {
        up,
        right,
        down,
        left
    }
}