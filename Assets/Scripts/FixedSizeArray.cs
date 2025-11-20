using System;
using System.Collections;
using System.Collections.Generic;

// TODO: move to ReplicaEngine namespace?
// TODO: make iterable?
public class FixedSizeArray<T> : IEnumerable<T> {
    const int LINEAR_SEARCH_CUTOFF = 16;

    public int Capacity { get; }
    public int Count => _count;

    readonly T[] _items;
    int _count;
    IComparer<T> _comparator;
    bool _sorted;


    public FixedSizeArray(int capacity) {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");

        Capacity = capacity;
        _items = new T[capacity];
        _count = 0;

        _sorted = false;
    }

    public FixedSizeArray(int capacity, IComparer<T> comparator) {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");

        Capacity = capacity;
        _items = new T[capacity];
        _count = 0;
        _comparator = comparator;

        _sorted = false;
    }

    public void Add(T item) {
        if (_count >= Capacity)
            throw new InvalidOperationException("Collection is full, cannot add more elements.");

        _items[_count] = item;
        _count++;
        _sorted = false;
    }

    public bool Remove(T item, bool ignoreComparator) {
        int index = Find(item, ignoreComparator);
        if (index != -1) {
            Remove(index);
            return true;
        }
        return false;
    }

    public void Remove(int index) {
        if (index >= _count || index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        _count--;
        if (index < _count) {
            Array.Copy(_items, index + 1, _items, index, _count - index);
        }

        _items[_count] = default;
    }

    public T RemoveLast() {
        T item = default;
        if (_count > 0) {
            _count--;
            item = _items[_count];
            _items[_count] = default;
        }
        return item;
    }

    // swap element with the last element (helps quick removal if called before 'RemoveLast')
    public void SwapWithLast(int index) {
        if (_count > 0 && index < _count - 1) {
            (_items[index], _items[_count - 1]) = (_items[_count - 1], _items[index]);
            _sorted = false;
        }
    }

    public void Clear() {
        if (typeof(T).IsClass) {
            Array.Clear(_items, 0, _count);
        }
        _count = 0;
        _sorted = false;
    }

    public T this[int index] {
        get {
            if (index >= _count || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            return _items[index];
        }
        set {
            if (index >= _count || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            _items[index] = value;
        }
    }

// TODO: OK?
//    public ReadOnlyCollection<T> GetArray() {
//        return _items.AsReadOnly();
//    }
    public IReadOnlyList<T> GetArray() {
        return _items;
    }

    public int Find(T item, bool ignoreComparator) {
        int index = -1;

        if (_sorted && _count > LINEAR_SEARCH_CUTOFF) {
            if (!ignoreComparator && _comparator != null) {
                index = Array.BinarySearch(_items, item, _comparator);
                //index = _items.BinarySearch(item, _comparator);
            } else {
                // TODO: check T implements 'IComparable'!?
                index = Array.BinarySearch(_items, item);
                //index = _items.BinarySearch(item);
            }
            if (index < 0) {
                index = -1;
            }
        } else {
            // linear search
            if (!ignoreComparator && _comparator != null) {
                for (int i = 0; i < _count; ++i) {
                    int result = _comparator.Compare(_items[i], item);
                    if (result == 0) {
                        index = i;
                        break;
                    } else if (result > 0 && _sorted) {
                        break;
                    }
                }
            } else {
                index = Array.IndexOf(_items, item);
            }
        }

        return index;
    }

    public void Sort(bool forceResort) {
        if (!_sorted || forceResort) {
            if (_comparator != null) {
                Array.Sort(_items, _comparator);
            } else {
                // TODO: check T implements 'IComparable'!?
                Array.Sort(_items);
            }
            _sorted = true;
        }
    }

    public int GetCount() {
        return _count;
    }

    public int GetCapacity() {
        return Capacity;
    }

    public void SetComparator(IComparer<T> comparator) {
        _comparator = comparator;
        _sorted = false;
    }

    public IEnumerator<T> GetEnumerator() {
        for (int i = 0; i < _count; i++) {
            yield return _items[i];
        }
    }

    // non-generic GetEnumerator (required by the non-generic IEnumerable interface)
    IEnumerator IEnumerable.GetEnumerator() {
        // Call the generic version
        return GetEnumerator();
    }
}
