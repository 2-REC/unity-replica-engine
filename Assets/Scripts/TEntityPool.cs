/* TEntityPool is a generic version of EntityPool that automatically casts to type T on allocation.
 */

using System;
using System.Collections.Generic;

// TODO: need to be derived from BaseObject!?!?

namespace ReplicaEngine {

    public abstract class TEntityPool<T> where T : class {
    //public abstract class TEntityPool<T> : BaseEntity where T : class {

        // TODO: rename properties
        public int CountAll { get; private set; }
        // was 'getAllocatedCount' (OR _size - countInactive ?)
        public int CountActive => CountAll - CountInactive;
        public int CountInactive => _pool.Count;


        private const int DEFAULT_SIZE = 32;

        // TODO: Stack for performance in push/pop
        private readonly Stack<T> _pool;
        private readonly int _size;

        //private readonly Action<T> _actionOnDestroy;



        public TEntityPool() {
            _size = DEFAULT_SIZE;
            _pool = new Stack<T>(_size);
            Fill();
        }

        public TEntityPool(int size) {
            _size = size;
            _pool = new Stack<T>(_size);
            Fill();
        }

        // TODO: needed?
        //public override void Reset() { }

        public T Allocate() {
            if (_pool.Count == 0) {
                // TODO: type?
                throw new Exception($"Entity pool of type {GetType().Name} exhausted!");
            }

            T item = _pool.Pop();

            OnAllocate(item);
            return item;
        }

        public void Release(T element) {
            OnRelease(element);

            if (_pool.Count >= _size) {
                //Exception
                //$"Entity pool of type {GetType().FullName} is full!!"
                return; // remove
            }
            _pool.Push(element);
        }

        public void Clear() {
            // TODO: if needed, function/action must be set somehow... (setter method)
            /*
            if (_actionOnDestroy != null) {
                foreach (T item in _pool) {
                    _actionOnDestroy(item);
                }
            }
            */
            _pool.Clear();
            CountAll = 0;
        }

        public void Dispose() {
            Clear();
            // TODO: more to do?
        }

        // TODO: CHECK!
        protected virtual void Fill() {
            Clear();
            for (int i = 0; i < _size; i++) {
                // TODO: ok? (if not, check 'T CreateItem', and 'GameComponent' type in derived class)
                //T item = CreateItem();
                T item = (T)CreateItem();
                _pool.Push(item);
                CountAll++;
            }

        }

        // TODO: needed?
        protected int GetSize() {
            return _size;
        }

        // TODO: not needed => was only used in 'Fill' in derived classes
        /*
        protected FixedSizeArray<Object> getAvailable() {
            return _pool;
        }
        */

        // TODO: 'object' ok? (if not, use T and specify type in derived classes)
        //protected abstract T CreateItem();
        protected abstract object CreateItem();
        // TODO: make abstract? (or keep empty virtual?)
        protected virtual void OnAllocate(T item) { }
        // TODO: 'object' ok? (if not, use T and specify type in derived classes)
        //protected void OnRelease(T item) { }
        protected virtual void OnRelease(object item) { }

    }

}
