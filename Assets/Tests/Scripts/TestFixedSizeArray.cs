using ReplicaEngine;
using System.Collections.Generic;
using UnityEngine;

// TODO: add more tests!
// + iterator tests

public class ImpEntity : PhasedEntity {
}


public class TestFixedSizeArray : MonoBehaviour {

    private class PhasedEntityComparator : IComparer<BaseEntity> {
        public int Compare(BaseEntity object1, BaseEntity object2) {
            int result = 0;
            if (object1 != null && object2 != null) {
                result = ((PhasedEntity)object1).phase - ((PhasedEntity)object2).phase;
            } else if (object1 == null && object2 != null) {
                result = 1;
            } else if (object2 == null && object1 != null) {
                result = -1;
            }
            return result;
        }
    }


    FixedSizeArray<BaseEntity> array;

    private void Awake() {
        // no comparator
        array = new FixedSizeArray<BaseEntity>(64);
        ////
        /*
        // ctor with comparator
        var comparator = new PhasedEntityComparator();
        array = new FixedSizeArray<BaseEntity>(64, comparator);
        */
        ////
        /*
        // set comparator
        array = new FixedSizeArray<BaseEntity>(64);
        var comparator = new PhasedEntityComparator();
        array.SetComparator(comparator);
        */
        ////

        //Debug.Log("count: " + array.GetCount());
        //Debug.Log("capacity: " + array.GetCapacity());

        //TestAdd();
        //Debug.Log("count after add: " + array.GetCount());

        //TestAccess();

        //Debug.Log("READ ONLY:");
        //var readonlyarray = array.GetArray();
        //foreach (BaseEntity i in  readonlyarray) {
        //    Debug.Log(i);
        //}

        //TestRemoveLast();

        //TestAdd();
        //TestRemove();

        //TestAdd();
        //TestRemoveNoComp();

        //TestAdd();
        //TestClear();

        TestAdd();
        TestSwap();

        TestFind();

        TestSort();

    }


    void TestAdd() {
        var obj = new ImpEntity();
        array.Add(obj);
        array.Add(new ImpEntity());
        array.Add(new PhasedEntity());

        var phobj = new PhasedEntity {
            phase = 5
        };
        array.Add(phobj);
    }

    string Dump() {
        string res = " ";
        for (int i = 0; i < array.GetCount(); ++i) {
            res += array[i] + ", ";
        }
        return res;
    }

    string DumpPhased() {
        string res = " ";
        for (int i = 0; i < array.GetCount(); ++i) {
            res += ((PhasedEntity)array[i]).phase + ", ";
        }
        return res;
    }

    void TestAccess() {
        string res = Dump();
        Debug.Log("Content:" + res);

        var p2 = new PhasedEntity {
            phase = 10
        };
        array[1] = p2;

        res = Dump();
        Debug.Log("Content after change [1]:" + res);

        Debug.Log("PHASE[1]:" + ((PhasedEntity)array[1]).phase);
    }

    void TestRemoveLast() {
        while (array.Count > 0) {
            array.RemoveLast();
            string res = Dump();
            Debug.Log("Content after remove last: " + res);
        }
        array.RemoveLast();
    }

    void TestRemove() {
        while (array.Count > 0) {
            array.Remove(0);
            string res = Dump();
            Debug.Log("Content after remove first: " + res);
        }
    }

    void TestRemoveNoComp() {
        while (array.Count > 0) {
            int mid = array.Count / 2;
            array.Remove(mid);
            string res = Dump();
            Debug.Log("Content after remove mid: " + res);
        }
    }

    void TestClear() {
        array.Clear();
        string res = Dump();
        Debug.Log("Content after clear: " + res);
    }

    void TestSwap() {
        array.SwapWithLast(1);
        string res = Dump();
        Debug.Log("Content after swap 1: " + res);
    }

    void TestFind() {
        var phased = new PhasedEntity {
            phase = 25
        };
        int index = array.Find(phased, false);
        Debug.Log("Item not found at: " + index);

        array[2] = phased;

        index = array.Find(phased, false);
        Debug.Log("Item found at: " + index);
    }

    void TestSort() {
        string res = DumpPhased();
        Debug.Log("Content before sort: " + res);

        array.Sort(false);
        res = DumpPhased();
        Debug.Log("Content after sort: " + res);
    }

}
