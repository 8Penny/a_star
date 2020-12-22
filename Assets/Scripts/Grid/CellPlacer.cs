using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Pooling;

namespace Grid {
    public class CellPlacer {
        private readonly Pathfinding _pathfinding;
        private List<int> _pathIndices;
        
        private readonly CellContainer _cells;
        
        private readonly CellPresenter[] _cPresenters;
        private CellPresenter _draggingCell;
        private CellPresenter _startCell;
        private CellPresenter _finishCell;
        
        private ObjectPool _objectPool;
        private readonly CellView[] _cellViews;

        private readonly int _cellCount;

        public CellPlacer(CellContainer cells, int startCellId, int finishCellId) {
            _cells = cells;
            _cellCount = _cells.cellCount;
            _cPresenters = new CellPresenter[_cellCount];

            for (var i = 0; i < _cellCount; i++) {
                var presenter = new CellPresenter(_cells[i]);
                presenter.ChangeVisibility += UpdateCellVisibility;
                _cPresenters[i] = presenter;
            }

            _startCell = _cPresenters[startCellId];
            _finishCell = _cPresenters[finishCellId];
            _pathfinding = new Pathfinding(_cells);
            _pathIndices = new List<int>();
            
            _cellViews = new CellView[_cellCount];
        }

        public void SetObjectPool(ObjectPool objectPool) {
            _objectPool = objectPool;
        }

        public void GenerateCellGrid() {
            for (var i = 0; i < _cellCount; i++) {
                _cPresenters[i].Start();
            }
        }

        public void UpdatePath() {
            for (var i = 0; i < _pathIndices.Count; i++) {
                if (_cPresenters[_pathIndices[i]].cellType == CellType.Path) {
                   _cPresenters[_pathIndices[i]].UpdateType(CellType.Free); 
                }
            }

            _pathIndices = _pathfinding.FindPath(_startCell.cellIndex, _finishCell.cellIndex);
            for (var i = 0; i < _pathIndices.Count; i++) {
                _cPresenters[_pathIndices[i]].UpdateType(CellType.Path);
            }
        }

        public void RevertCellType(Vector2 position) {
            var cellIndex = _cells.GetCellIndex(position);
            _cPresenters[cellIndex].InvertType();
            UpdatePath();
        }

        public void TryTakeDraggableCell(Vector2 position) {
            var cellIndex = _cells.GetCellIndex(position);

            if (cellIndex == _startCell.cellIndex) {
                _draggingCell = _startCell;
                return;
            }

            if (cellIndex == _finishCell.cellIndex) {
                _draggingCell = _finishCell;
            }
        }

        public void TryChangeDraggableCellPosition(Vector2 position) {
            if (_draggingCell == null) {
                return;
            }

            position = new Vector2(position.x, -position.y);
            _draggingCell.UpdatePosition(position);
        }

        public void TryPlaceDraggableCell(Vector2 position) {
            if (_draggingCell == null) {
                return;
            }

            var indexToPut = _cells.GetCellIndex(position);
            var isCorrectIndex = indexToPut != -1;
            if (isCorrectIndex && !_cells[indexToPut].isBusy) {
                var draggingCellType = _draggingCell.cellType;
                _cPresenters[indexToPut].UpdateType(draggingCellType);
                _cPresenters[_draggingCell.cellIndex].UpdateType(CellType.Free);

                if (draggingCellType == CellType.Start) {
                    _startCell = _cPresenters[indexToPut];
                }
                else {
                    _finishCell = _cPresenters[indexToPut];
                }
            }

            _draggingCell.ResetPosition();
            _draggingCell = null;
            UpdatePath();
        }

        private void UpdateCellVisibility(int index, bool isVisible) {
            if (isVisible) {
                var view = _objectPool.GetObject();
                _cellViews[index] = view;
                view.SetPresenter(_cPresenters[index]);
                _cPresenters[index].ResetPosition();
            }
            else {
                var currentView = _cellViews[index];
                if (currentView == null) {
                    return;
                }

                _objectPool.PoolObject(currentView);
                _cellViews[index] = null;
            }
        }

        public void Disable() {
            for (var i = 0; i < _cellCount; i++) {
                _cPresenters[i].ChangeVisibility -= UpdateCellVisibility;
            }
        }
    }
}