using UnityEngine;
using System;

namespace Grid {
    public class CellContainer {
        private readonly int _columnCount;
        private readonly int _rowCount;
        private readonly int _cellSize;
        private readonly int _arraySize;

        private readonly Cell[] _cells;
        public int cellCount => _arraySize;

        public CellContainer(int columns, int rows, int cSize) {
            _cellSize = cSize;
            _columnCount = columns;
            _rowCount = rows;
            _arraySize = columns * rows;

            _cells = new Cell[_arraySize];
            for (var i = 0; i < _arraySize; i++) {
                _cells[i] = new Cell(i, GetPosition(i) * _cellSize);
            }
        }

        public int[] GetNeighbors(Cell cell) {
            var result = new int[4];

            var planePosition = cell.position / _cellSize;
            planePosition.y = Math.Abs(planePosition.y);

            var hasUp = planePosition.y != 0;
            var hasDown = planePosition.y < _rowCount - 1;
            var hasLeft = planePosition.x != 0;
            var hasRight = planePosition.x < _columnCount - 1;

            var up = hasUp ? (planePosition.y - 1) * _columnCount + planePosition.x : -1;
            var down = hasDown ? (planePosition.y + 1) * _columnCount + planePosition.x : -1;
            var left = hasLeft ? planePosition.y * _columnCount + planePosition.x - 1 : -1;
            var right = hasRight ? planePosition.y * _columnCount + planePosition.x + 1 : -1;

            result[0] = (int) up;
            result[1] = (int) down;
            result[2] = (int) left;
            result[3] = (int) right;

            return result;
        }

        private Vector2 GetPosition(int index) {
            var row = index / _columnCount;
            var column = index % _columnCount;
            return new Vector2(column, -row);
        }

        public int GetCellIndex(Vector2 position) {
            position /= _cellSize * 1.0f;
            var index = Math.Floor(position.y) * _columnCount + Math.Floor(position.x);
            if (index >= _arraySize || index < 0) {
                return -1;
            }

            return (int) index;
        }

        public Cell this[int i] => _cells[i];
    }
}