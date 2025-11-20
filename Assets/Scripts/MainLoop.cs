/* Main game loop.
 * Updates the time system and passes the result down to the rest of the game graph.
 * This entity is effectively the root of the game graph.
 */

namespace ReplicaEngine {

    public class MainLoop : EntityManager {

        private TimeSystem mTimeSystem;


        // TODO: make sure it calls base ctor
        // ensures that time updates before everything else.
        public MainLoop() {
            mTimeSystem = new TimeSystem();
            systemRegistry.timeSystem = mTimeSystem;
            systemRegistry.RegisterForReset(mTimeSystem);
        }
        /*
        void Awake() {
            mTimeSystem = gameObject.AddComponent<TimeSystem>();
            sSystemRegistry.timeSystem = mTimeSystem;
            sSystemRegistry.RegisterForReset(mTimeSystem);
        }
        */

        public override void CustomUpdate(float timeDelta, BaseEntity parent) {
            mTimeSystem.CustomUpdate(timeDelta, parent);
            float newTimeDelta = mTimeSystem.GetFrameDelta(); // the time system may warp time.
            base.CustomUpdate(newTimeDelta, parent);
        }
    }
}
