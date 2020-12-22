using System;

namespace Utils {
    public class PriorityQueue {
        private HeapElement[] _heap = new HeapElement[1000];
        private int _lastHeapIndex = -1;

        public bool isEmpty => _lastHeapIndex == -1;

        public void Clear() {
            _lastHeapIndex = -1;
        }

        public void Insert(int index, int priority) {
            var element = new HeapElement(index, priority);
            _heap[++_lastHeapIndex] = element;
            SiftUp(_lastHeapIndex);
        }

        public int ExtractMax() {
            if (isEmpty) {
                return -1;
            }

            var result = _heap[0].index;
            _heap[0] = _heap[_lastHeapIndex];
            --_lastHeapIndex;
            SiftDown(0);
            return result;
        }

        private void SiftUp(int elementIndex) {
            while (elementIndex > 0 && _heap[Parent(elementIndex)] < _heap[elementIndex]) {
                Swap(elementIndex, Parent(elementIndex));
                elementIndex = Parent(elementIndex);
            }
        }

        private void SiftDown(int elementIndex) {
            var leftChild = 2 * elementIndex + 1;
            var rightChild = 2 * elementIndex + 2;

            var isLeftChildExist = leftChild < _lastHeapIndex;
            var isRightChildExist = rightChild < _lastHeapIndex;

            var maxElementIndex = elementIndex;

            if (isLeftChildExist && _heap[leftChild] > _heap[maxElementIndex]) {
                maxElementIndex = leftChild;
            }

            if (isRightChildExist && _heap[rightChild] > _heap[maxElementIndex]) {
                maxElementIndex = rightChild;
            }

            if (maxElementIndex == elementIndex) {
                return;
            }

            Swap(maxElementIndex, elementIndex);
            SiftDown(maxElementIndex);
        }

        private void Swap(int ind1, int ind2) {
            var t = _heap[ind2];
            _heap[ind2] = _heap[ind1];
            _heap[ind1] = t;
        }

        private static int Parent(int elementIndex) {
            if (elementIndex == 0) {
                return -1;
            }

            var parentIndex = (int) Math.Floor((elementIndex - 1) / 2.0f);
            return parentIndex;
        }
    }

    public class HeapElement {
        private readonly int _index;
        private readonly int _priority;

        public int index => _index;

        public HeapElement(int i, int ePriority) {
            _index = i;
            _priority = ePriority;
        }

        public static bool operator >(HeapElement h1, HeapElement h2) {
            return h1._priority > h2._priority;
        }

        public static bool operator <(HeapElement h1, HeapElement h2) {
            return h1._priority < h2._priority;
        }
    }
}