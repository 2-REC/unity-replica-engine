/* A basic entity that adds an execution phase.
 * When PhasedEntities are combined with PhasedEntityManagers,
 * entities within the manager will be updated by phase.
 */

/*
//using UnityEngine;

namespace ReplicaEngine {

    public class PhasedEntity : BaseEntity {

        private GameComponent gameComponent;

        // TODO: make read-only property? (if no impact on perf)
        // TODO: initial value?
        public int phase;


        // TODO: make sure it calls base ctor
        public PhasedEntity() {}

        // TODO: ?
        public void SetComponent(GameComponent gameComponent) {
            this.gameComponent = gameComponent;
        }

        public void SetPhase(int phaseValue) {
            phase = phaseValue;
        }

        public override void Reset() {
            gameComponent.Reset();
        }

        public override void CustomUpdate(float timeDelta, BaseEntity parent) {
            gameComponent.CustomUpdate(timeDelta, parent);
        }

    }

}
*/

//using UnityEngine;

namespace ReplicaEngine {

    public class PhasedEntity : BaseEntity {

        // TODO: make read-only property? (if no impact on perf)
        // TODO: initial value?
        public int phase;


        // TODO: make sure it calls base ctor
        public PhasedEntity() { }

        public override void Reset() {}

        public void SetPhase(int phaseValue) {
            phase = phaseValue;
        }

    }

}
