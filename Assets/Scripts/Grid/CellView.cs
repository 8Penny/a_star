using UnityEngine;
using UnityEngine.UI;

namespace Grid {
    public class CellView : MonoBehaviour {
        [SerializeField] private RectTransform _transform;
        [SerializeField] private Image _image;

        private CellPresenter _presenter;

        public void SetPresenter(CellPresenter presenter) {
            ForgetOldPresenter();
            _presenter = presenter;
            _presenter.UpdateAction += OnUpdate;
        }

        private void OnUpdate() {
            ChangeColor();
            ChangePosition();
        }

        private void ChangeColor() {
            _image.color = _presenter.color;
        }

        private void ChangePosition() {
            _transform.anchoredPosition = _presenter.position;
        }

        private void OnDisable() {
            ForgetOldPresenter();
        }

        private void ForgetOldPresenter() {
            if (_presenter != null) {
                _presenter.UpdateAction -= OnUpdate;
            }
        }
    }
}