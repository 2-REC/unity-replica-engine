/* A game component implements a single feature of a game entity.
 * Components are run once per frame when their parent entity is active.
 * Updating a game entity is equivalent to updating all of its components.
 * Note that a game entity may contain more than one instance of the same type of component.
 */

/*
using UnityEngine;

namespace ReplicaEngine {

    //public abstract class GameComponent : PhasedEntity {
    public abstract class GameComponent : MonoBehaviour {

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
        // TODO: protected?
        protected PhasedEntity phasedEntity;


        protected virtual void Awake() {
            shared = false;

            // TODO: get GameComponent here? (should be allocated at same time as this)
            // + need to link both ways... phasedEntity.SetComponent(this)
            // ... more?

        }

        public void SetPhase(int phaseValue) {
            phasedEntity.SetPhase(phaseValue);
        }

        public abstract void Reset();

        // TODO: ok?
        public virtual void CustomUpdate(float timeDelta, BaseEntity parent) { }

    }

}
*/

using System;
using UnityEngine;

namespace ReplicaEngine {

    //public abstract class GameComponent : PhasedEntity {
    public class GameComponent : PhasedEntity {

        private GameComponentBridge implementedComponent;

        public Type componentType;

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


        protected virtual void Awake() {
            shared = false;

            // TODO: get GameComponent here? (should be allocated at same time as this)
            // + need to link both ways... phasedEntity.SetComponent(this)
            // ... more?

        }

        //public abstract void Reset();
        public override void Reset() {
            implementedComponent.Reset();
        }


        // TODO: ok?
        //public virtual void CustomUpdate(float timeDelta, BaseEntity parent) { }
        public override void CustomUpdate(float timeDelta, BaseEntity parent) {
            implementedComponent.CustomUpdate(timeDelta, parent);
        }

        // TODO: change name!
        public void SetComponent(GameComponentBridge component) {
            implementedComponent = component;
            // TODO ?
            componentType = component.GetType();
        }

    }

}
