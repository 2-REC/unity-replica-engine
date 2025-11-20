using System.Collections.Generic;

namespace ReplicaEngine {

    public abstract class Sorter<T> {
        public abstract void Sort(T[] array, int count, IComparer<T> comparator);

    }

}
