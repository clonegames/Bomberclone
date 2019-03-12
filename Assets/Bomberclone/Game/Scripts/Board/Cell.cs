using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberclone {

    public class Cell {

        public enum CellType {
            Empty = 0,
            HardWall = 1,
            SoftWall = 2,
            Bomb = 3,
            Powerup = 4
        }

        public Vector3 Position {
            get;
            private set;
        }

        public CellType Type {
            get;
            private set;
        }

        public int Id {
            get;
            private set;
        }

        private GameObject m_go;

        public Cell(GameObject prefab, Vector3 position, int id, CellType type, Transform parent) {
            if (prefab != null) {
                m_go = GameObject.Instantiate(prefab, position, Quaternion.identity, parent);
            }
            Position = position;
            transform.localScale = Vector3.one / (MapManager.Instance.BoardSize * 10);
            Id = id;
            Type = type;
        }
    }
}