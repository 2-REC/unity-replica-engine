/* The core object from which most other objects are derived.
 * Anything that will be managed by an EntityManager,
 * and anything that requires an update per frame should be derived from BaseEntity.
 * BaseEntity also defines the interface for the entity-wide system registry.
 */

// TODO: make comparable (IComparer)? (or sure it will always be done explicitely by derived class?)

//using UnityEngine;

namespace ReplicaEngine {

    //public abstract class BaseEntity : MonoBehaviour {
    public abstract class BaseEntity {
        // TODO: OK for making sure it's a singleton?
        readonly static public SystemRegistry systemRegistry = new();


        public BaseEntity() {}
        //protected virtual void Awake() {}
        /*
        protected virtual void Init() {}
        public static BaseEntity Create(GameObject targetObject) {
            BaseEntity baseEntity = targetObject.AddComponent<BaseEntity>();
            baseEntity.Init();
            return baseEntity;
        }
        */

        // TODO: can be renamed to 'Update'
        //public virtual void Update(float timeDelta, BaseEntity parent) {}
        public virtual void CustomUpdate(float timeDelta, BaseEntity parent) {}

        public abstract void Reset();

    }

}
