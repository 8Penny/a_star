using UnityEngine;

namespace Grid {
    public class Cell {
        private readonly int _index;
        private readonly Vector2 _position;
        private CellType _type;

        public int index => _index;
        public Vector2 position => _position;
        public CellType type => _type;
        public bool isBusy => type == CellType.Busy;
        public bool isAvailableToReplace => type == CellType.Free || type == CellType.Path;

        public Cell(int ind, Vector2 pos) {
            _index = ind;
            _position = pos;
            _type = CellType.Free;
        }

        public void SetType(CellType t) {
            _type = t;
        }
    }

    public enum CellType {
        Free,
        Busy,
        Start,
        Finish,
        Path
    }
}