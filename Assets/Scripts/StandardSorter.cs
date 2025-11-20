using System;
using System.Collections.Generic;

namespace ReplicaEngine {

    public class StandardSorter<T> : Sorter<T> {

        public override void Sort(T[] array, int count, IComparer<T> comparator) {
            Array.Sort(array, 0, count, comparator);
        }
    }

}