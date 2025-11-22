/* A derivation of EntityManager that sorts its children if they are of type PhasedEntity.
 * Sorting is performed on add.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReplicaEngine {

    public class PhasedEntityManager : EntityManager {

        static readonly PhasedEntityComparator sPhasedEntityComparator = new();
        bool mDirty;
        readonly PhasedEntity mSearchDummy;


        // TODO: make sure it calls base ctor
        public PhasedEntityManager() {
            mDirty = false;
            GetEntities().SetComparator(sPhasedEntityComparator);
            GetPendingEntities().SetComparator(sPhasedEntityComparator);
            mSearchDummy = new PhasedEntity();
        }

        public PhasedEntityManager(int arraySize) : base(arraySize) {
            mDirty = false;
            GetEntities().SetComparator(sPhasedEntityComparator);
            GetPendingEntities().SetComparator(sPhasedEntityComparator);
            mSearchDummy = new PhasedEntity();
        }

        /*
        protected override void Awake() {
            // TODO: problem: arrays will be created here, and maybe recreated in 'Create'...
            base.Awake();
            mDirty = false;
            GetEntities().SetComparator(sPhasedEntityComparator);
            GetPendingEntities().SetComparator(sPhasedEntityComparator);
            mSearchDummy = gameObject.AddComponent<PhasedEntity>();
        }
        */
        /*
        protected override void Init(int arraySize = DEFAULT_ARRAY_SIZE) {
            base.Init(arraySize);
            mDirty = false;
            GetEntities().SetComparator(sPhasedEntityComparator);
            GetPendingEntities().SetComparator(sPhasedEntityComparator);
            mSearchDummy = gameObject.AddComponent<PhasedEntity>();
        }
        // TODO: 'new' ok? (if not, base can't be static)
        public static new PhasedEntityManager Create(GameObject targetObject, int arraySize=DEFAULT_ARRAY_SIZE) {
            PhasedEntityManager phasedEntityManager = targetObject.AddComponent<PhasedEntityManager>();
            phasedEntityManager.Init(arraySize);
            return phasedEntityManager;
        }
        */

        public override void CommitUpdates() {
            base.CommitUpdates();
            if (mDirty) {
                GetEntities().Sort(true);
                mDirty = false;
            }
        }

        public override void Add(BaseEntity item) {
            if (item is PhasedEntity) {
                base.Add(item);
                mDirty = true;
            } else {
                // TODO: exception type? (or assert?)
                throw new Exception("Can't add a non-PhasedEntity to a PhasedEntityManager!");
            }
        }

        public BaseEntity Find(int phase) {
            mSearchDummy.SetPhase(phase);
            int index = GetEntities().Find(mSearchDummy, false);
            BaseEntity result = null;
            if (index != -1) {
                result = GetEntities()[index];
            } else {
                index = GetPendingEntities().Find(mSearchDummy, false);
                if (index != -1) {
                    result = GetPendingEntities()[index];
                }
            }
            return result;
        }

    }


    class PhasedEntityComparator : IComparer<BaseEntity> {
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

}
