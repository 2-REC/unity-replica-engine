/* A game component implements a single feature of a game entity.
 * Components are run once per frame when their parent entity is active.
 * Updating a game entity is equivalent to updating all of its components.
 * Note that a game entity may contain more than one instance of the same type of component.
 */

//using UnityEngine;

namespace ReplicaEngine {

    public abstract class GameComponent : PhasedEntity {

        // defines high-level buckets within which components may choose to run
        public enum ComponentPhases {
            THINK,                  // decisions are made
            PHYSICS,                // impulse velocities are summed
            POST_PHYSICS,           // inertia, friction, and bounce
            MOVEMENT,               // position is updated
            COLLISION_DETECTION,    // intersections are detected
            COLLISION_RESPONSE,     // intersections are resolved
            POST_COLLISION,         // position is now final for the frame
            ANIMATION,              // animations are selected
            PRE_DRAW,               // drawing state is initialized
            DRAW,                   // drawing commands are scheduled
            FRAME_END,              // final cleanup before the next update
        }

        public bool shared;


        // TODO: make sure it calls base ctor
        public GameComponent() {
            shared = false;
        }
        /*
        protected override void Awake() {
            // TODO: needed? (doesn't do anything)
            //base.Awake();
            shared = false;
        }
        */
        /*
        protected override void Init() {
            base.Init();
        }
        public static new GameComponent Create(GameObject targetObject) {
            GameComponent gameComponent = targetObject.AddComponent<GameComponent>();
            gameComponent.Init();
            return gameComponent;
        }
        */

    }

}
