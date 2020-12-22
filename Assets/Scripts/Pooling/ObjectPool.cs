using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Pooling {
    public class ObjectPool : MonoBehaviour {
        [SerializeField] private Transform _parentTransform;
        [SerializeField] private GameObject _objectPrefab;
        [SerializeField] private int _poolSize = 100;

        private List<CellView> _pooledViews;

        public void Load() {
            _pooledViews = new List<CellView>();
            for (var i = 0; i < _poolSize; i++) {
                _pooledViews.Add(InstantiateGameObject(false));
            }
        }

        public CellView GetObject() {
            for (var i = 0; i < _pooledViews.Count; i++) {
                var currentView = _pooledViews[i];
                if (currentView.gameObject.activeInHierarchy) {
                    continue;
                }
                currentView.gameObject.SetActive(true);
                return currentView;
            }

            return AddExtraObject();
        }

        private CellView AddExtraObject() {
            var newView = InstantiateGameObject(true);
            _pooledViews.Add(newView);
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
        }
    }
}