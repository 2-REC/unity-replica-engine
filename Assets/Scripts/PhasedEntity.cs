/* A basic entity that adds an execution phase.
 * When PhasedEntities are combined with PhasedEntityManagers,
 * entities within the manager will be updated by phase.
 */

//using UnityEngine;

namespace ReplicaEngine {

    public class PhasedEntity : BaseEntity {

        // TODO: make read-only property? (if no impact on perf)
        // TODO: initial value?
        public int phase;


        // TODO: make sure it calls base ctor
        public PhasedEntity() {}
        /*
        protected override void Awake() {
            base.Awake();
        }
        */
        /*
        protected override void Init() {
            base.Init();
        }
        public static new PhasedEntity Create(GameObject targetObject) {
            PhasedEntity phasedEntity = targetObject.AddComponent<PhasedEntity>();
            phasedEntity.Init();
            return phasedEntity;
        }
        */

        public override void Reset() {}

        public void SetPhase(int phaseValue) {
            phase = phaseValue;
        }

    }

}
