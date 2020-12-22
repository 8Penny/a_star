using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Utils {
    public class Pathfinding {
        private PathObject[] _pathCells;
        private CellContainer _container;
        private PriorityQueue _priorityQueue;

        private PathObject _finishCell;
        private PathObject _startCell;

        public Pathfinding(CellContainer cells) {
            var cellCount = cells.cellCount;
            _pathCells = new PathObject[cellCount];
            for (var i = 0; i < cellCount; i++) {
                _pathCells[i] = new PathObject(cells[i]);
            }

            _container = cells;
            _priorityQueue = new PriorityQueue();
        }

        public List<int> FindPath(int startCellInd, int finishCellIndex) {
            _startCell = _pathCells[startCellInd];
            _startCell.costFromStart = 0;

            _finishCell = _pathCells[finishCellIndex];
            _priorityQueue.Insert(_startCell.index, 0);

            while (!_priorityQueue.isEmpty) {
                FillCellInfo();
            }

            var path = GetCalculatedPath();

            ResetPathObjects();

            return path;
        }

        private void FillCellInfo() {
            var currentCellIndex = _priorityQueue.ExtractMax();
            var currentCell = _pathCells[currentCellIndex];
            if (currentCell.index == _finishCell.index) {
                return;
            }

            foreach (var nCellInd in _container.GetNeighbors(currentCell.cell)) {
                if (nCellInd < 0) {
                    continue;
                }

                if (_pathCells[nCellInd].cell.isBusy) {
                    continue;
                }

                var nCell = _pathCells[nCellInd];
                var cost = currentCell.costFromStart + 1;
                var oldCost = nCell.costFromStart;
                if (oldCost < 0 || cost < oldCost) {
                    nCell.parentIndex = currentCellIndex;
                    nCell.costFromStart = cost;

                    var supposedDistanceToFinish = GetSupposedDistance(nCell.cell.position, _finishCell.cell.position);
                    var priority = -(cost + supposedDistanceToFinish);
                    _priorityQueue.Insert(nCellInd, priority);
                }
            }
        }

        private List<int> GetCalculatedPath() {
            var result = new List<int>();
            if (_finishCell.parentIndex == -1) {
                return result;
            }

            var currentIndex = _finishCell.index;
            while (true) {
                currentIndex = _pathCells[currentIndex].parentIndex;
                if (currentIndex == _startCell.index) {
                    break;
                }

                result.Add(currentIndex);
            }

            return result;
        }

        private static int GetSupposedDistance(Vector2 from, Vector2 to) {
            return (int) (Math.Abs(from.x - to.x) + Math.Abs(from.y - to.y));
        }

        private void ResetPathObjects() {
            _priorityQueue.Clear();
            for (var i = 0; i < _pathCells.Length; i++) {
                _pathCells[i].Reset();
            }
        }
    }

    public class PathObject {
        public Cell cell;
        public int costFromStart;
        public int parentIndex;
        public int index => cell.index;

        public PathObject(Cell c) {
            cell = c;
            Reset();
        }

        public void Reset() {
            costFromStart = -1;
            parentIndex = -1;
        }
    }
}