using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Pooling {
    public class ObjectPool : MonoBehaviour {
        [SerializeField] private Transform _parentTransform;
        [SerializeField] private GameObject _objectPrefab;
        [SerializeField] private int _poolSize = 100;

        private List<CellView> _pooledViews;
        private int _lastFreeIndex;

        public void Load() {
            _lastFreeIndex = _poolSize - 1;
            _pooledViews = new List<CellView>(new CellView[_poolSize]);

            for (var i = 0; i < _poolSize; i++) {
                _pooledViews[i] = InstantiateGameObject(false);
            }
        }

        public CellView GetObject() {
            if (_lastFreeIndex == -1) {
                return AddExtraObject();
            }

            var currentView = _pooledViews[_lastFreeIndex--];
            currentView.gameObject.SetActive(true);
            return currentView;
        }

        private CellView AddExtraObject() {
            var newView = InstantiateGameObject(true);
            return newView;
        }

        private CellView InstantiateGameObject(bool isVisible) {
            var newObject = Instantiate(_objectPrefab, _parentTransform);
            newObject.SetActive(isVisible);
            var view = newObject.GetComponent<CellView>();
            return view;
        }

        public void PoolObject(CellView view) {
            view.gameObject.SetActive(false);
            _pooledViews[++_lastFreeIndex] = view;
        }
    }
}