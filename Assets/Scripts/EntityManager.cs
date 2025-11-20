/* EntityManagers are "group nodes" in the game graph.
 * They contain child entities, and updating an entity manager invokes update on its children.
 * EntityManagers themselves are derived from BaseEntity, so they may be strung together into a hierarchy of entities.
 * EntityManager may be specialized to implement special types of traversals (e.g. PhasedEntityManager sorts its children).
 */

//using UnityEngine;

namespace ReplicaEngine {

    public class EntityManager : BaseEntity {

        protected const int DEFAULT_ARRAY_SIZE = 64;

        private readonly FixedSizeArray<BaseEntity> mEntities;
        private readonly FixedSizeArray<BaseEntity> mPendingAdditions;
        private readonly FixedSizeArray<BaseEntity> mPendingRemovals;


        // TODO: make sure it calls base ctor
        //public EntityManager() : base() {
        public EntityManager() {
            mEntities = new FixedSizeArray<BaseEntity>(DEFAULT_ARRAY_SIZE);
            mPendingAdditions = new FixedSizeArray<BaseEntity>(DEFAULT_ARRAY_SIZE);
            mPendingRemovals = new FixedSizeArray<BaseEntity>(DEFAULT_ARRAY_SIZE);
        }

        // TODO: make sure it calls base ctor (no param)
        //public EntityManager(int arraySize) : base() {
        public EntityManager(int arraySize) {
            mEntities = new FixedSizeArray<BaseEntity>(arraySize);
            mPendingAdditions = new FixedSizeArray<BaseEntity>(arraySize);
            mPendingRemovals = new FixedSizeArray<BaseEntity>(arraySize);
        }

        /*
        protected override void Awake() {
            // TODO: needed? (doesn't do anything)
            //base.Awake();
            EntityManagerInit(DEFAULT_ARRAY_SIZE);
        }
        */
        /*
        protected virtual void Init(int arraySize) {
            base.Init();
            mEntities = new FixedSizeArray<BaseEntity>(arraySize);
            mPendingAdditions = new FixedSizeArray<BaseEntity>(arraySize);
            mPendingRemovals = new FixedSizeArray<BaseEntity>(arraySize);
        }
        // TODO horrible... => allows to pass parameter (Init will be called twice...)
        public static EntityManager Create(GameObject targetObject, int arraySize=DEFAULT_ARRAY_SIZE) {
            EntityManager entityManager = targetObject.AddComponent<EntityManager>();
            entityManager.Init(arraySize);
            return entityManager;
        }
        */

        public override void Reset() {
            CommitUpdates();
            foreach (BaseEntity item in mEntities) {
                item.Reset();
            }
        }

        public virtual void CommitUpdates() {
            foreach (BaseEntity item in mPendingAdditions) {
                mEntities.Add(item);
            }
            mPendingAdditions.Clear();

            foreach (BaseEntity item in mPendingRemovals) {
                mEntities.Remove(item, true);
            }
            mPendingRemovals.Clear();
        }

        public override void CustomUpdate(float timeDelta, BaseEntity parent) {
            CommitUpdates();
            foreach (BaseEntity item in mEntities) {
                item.CustomUpdate(timeDelta, this);
            }
        }

        public FixedSizeArray<BaseEntity> GetEntities() {
            return mEntities;
        }

        public int GetCount() {
            return mEntities.GetCount();
        }

        public int GetConcreteCount() {
            return mEntities.GetCount() + mPendingAdditions.GetCount() - mPendingRemovals.GetCount();
        }

        public BaseEntity Get(int index) {
            return mEntities[index];
        }

        public virtual void Add(BaseEntity item) {
            mPendingAdditions.Add(item);
        }

        public virtual void Remove(BaseEntity item) {
            mPendingRemovals.Add(item);
        }

        public void RemoveAll() {
            foreach (BaseEntity item in mEntities) {
                mPendingRemovals.Add(item);
            }
            mPendingAdditions.Clear();
        }

        public T FindByClass<T>() where T : BaseEntity {
            foreach (BaseEntity item in mEntities) {
                /*
                if (item is T t) {
                    return t;
                }
                */
                // TODO: or? (strict check) => should be able to do both? (add 'strict' bool param)
                if (item.GetType() == typeof(T)) {
                    return (T)item;
                }
            }
            return default;
        }

        protected FixedSizeArray<BaseEntity> GetPendingEntities() {
            return mPendingAdditions;
        }

    }

}
