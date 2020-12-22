using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Systems {
    public class EventHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,
        IPointerDownHandler, IPointerUpHandler {
        [SerializeField] private RectTransform _rectTransform = new RectTransform();
        
        private Vector2 _downPoint;
        private bool _isDown;
        private bool _isDragging;

        public event Action<Vector2> OnClick;
        public event Action<Vector2> OnDraggingStarted;
        public event Action<Vector2> OnDragging;
        public event Action<Vector2> OnDraggingEnded;

        
        public void OnPointerClick(PointerEventData eventData) {
            if (_isDragging) {
                return;
            }
            
            OnClick?.Invoke(ConvertPosition(eventData.position));
        }

        public void OnBeginDrag(PointerEventData eventData) {
            OnDraggingStarted?.Invoke(_downPoint);
            _isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData) {
            OnDraggingEnded?.Invoke(ConvertPosition(eventData.position));
            _isDragging = false;
        }

        public void OnDrag(PointerEventData eventData) {
            OnDragging?.Invoke(ConvertPosition(eventData.position));
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (_isDown) {
                return;
            }

            _downPoint = ConvertPosition(eventData.position);
            _isDown = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            _isDown = false;
        }

        private Vector2 ConvertPosition(Vector2 eventPosition) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventPosition, null,
                out var localPosition);
            var result = new Vector2(localPosition.x, -localPosition.y);
            return result;
        }
    }
}