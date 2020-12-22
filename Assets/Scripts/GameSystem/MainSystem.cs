using UnityEngine;
using Grid;
using Pooling;

namespace GameSystem {
    public class MainSystem : MonoBehaviour {
        [SerializeField] private EventHandler _eventHandler;
        [SerializeField] private ObjectPool _objectPool;
        [SerializeField] private MazeData _maze;
        
        private const int ColumnCount = 70;
        private const int RowCount = 40;
        private const int CellSize = 10;

        private CellContainer _cells;
        private CellPlacer _cellPlacer;

        private void Awake() {
            _cells = new CellContainer(ColumnCount, RowCount, CellSize);
            LoadMazeData();
            _cellPlacer = new CellPlacer(_cells, _maze.startIndex, _maze.finishIndex);
            _objectPool.Load();
            _cellPlacer.SetObjectPool(_objectPool);
            
            _eventHandler.OnClick += _cellPlacer.RevertCellType;
            _eventHandler.OnDraggingStarted += _cellPlacer.TryTakeDraggableCell;
            _eventHandler.OnDragging += _cellPlacer.TryChangeDraggableCellPosition;
            _eventHandler.OnDraggingEnded += _cellPlacer.TryPlaceDraggableCell;

            _cellPlacer.GenerateCellGrid();
        }

        private void LoadMazeData() {
            _cells[_maze.startIndex].SetType(CellType.Start);
            _cells[_maze.finishIndex].SetType(CellType.Finish);
            for (var i = 0; i < _maze.busyIndices.Count; i++) {
                _cells[_maze.busyIndices[i]].SetType(CellType.Busy);
            }
        }

        private void Start() {
            _cellPlacer.UpdatePath();
        }
        
        private void OnDestroy() {
            _cellPlacer.Disable();
            _eventHandler.OnClick -= _cellPlacer.RevertCellType;
            _eventHandler.OnDraggingStarted -= _cellPlacer.TryTakeDraggableCell;
            _eventHandler.OnDragging -= _cellPlacer.TryChangeDraggableCellPosition;
            _eventHandler.OnDraggingEnded -= _cellPlacer.TryPlaceDraggableCell;
        }
    }
}