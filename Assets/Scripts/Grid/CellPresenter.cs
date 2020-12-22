using UnityEngine;

namespace Grid {
    public class CellPresenter {
        private Vector2 _position;
        private Color _color;
        private Cell _cell;

        private Color _busyColor = new Color(0.01f, 0.19f, 0.15f);
        private Color _finishColor = new Color(1, 0.26f, 0);
        private Color _startColor = new Color(1, 0.456f, 0);
        private Color _pathColor = new Color(0.8352f, 0.886f, 0.929f);

        public Vector2 position => _position;
        public Color color => _color;

        public delegate void UpdateHandler();
        public event UpdateHandler UpdateEvent;

        public delegate void CellVisibilityHandler(int index, bool isVisible);
        public event CellVisibilityHandler ChangeVisibility;

        public int cellIndex => _cell.index;
        public CellType cellType => _cell.type;


        public CellPresenter(Cell cell) {
            _cell = cell;
            _position = cell.position;
            UpdateColor();
        }

        public void Start() {
            ChangeVisibility?.Invoke(cellIndex, cellType != CellType.Free);
            UpdateEvent?.Invoke();
        }

        public void ResetPosition() {
            UpdatePosition(_cell.position);
        }

        public void UpdatePosition(Vector2 position) {
            _position = position;
            UpdateEvent?.Invoke();
        }

        public void InvertType() {
            switch (cellType) {
                case CellType.Finish:
                case CellType.Start:
                    return;
                case CellType.Busy:
                    UpdateType(CellType.Free);
                    return;
                default:
                    UpdateType(CellType.Busy);
                    break;
            }
        }

        public void UpdateType(CellType cType) {
            if (cType == cellType) {
                return;
            }

            var isVisibilityChanged = cType == CellType.Free || cellType == CellType.Free;

            _cell.SetType(cType);
            UpdateColor();

            if (isVisibilityChanged) {
                ChangeVisibility?.Invoke(_cell.index, cellType != CellType.Free);
            }

            UpdateEvent?.Invoke();
        }

        private void UpdateColor() {
            switch (_cell.type) {
                case CellType.Busy:
                    _color = _busyColor;
                    break;
                case CellType.Finish:
                    _color = _finishColor;
                    break;
                case CellType.Start:
                    _color = _startColor;
                    break;
                case CellType.Path:
                    _color = _pathColor;
                    break;
                default:
                    _color = Color.gray;
                    break;
            }
        }
    }
}