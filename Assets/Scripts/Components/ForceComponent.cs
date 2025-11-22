/*
namespace ReplicaEngine {

    public class ForceComponent : GameComponent {

        public GameComponent gameComponent;


        private const float DEFAULT_MASS = 1.0f;
        private const float DEFAULT_BOUNCINESS = 0.1f;
        private const float DEFAULT_STATIC_FRICTION_COEFFICIENT = 0.05f;
        private const float DEFAULT_DYNAMIC_FRICTION_COEFFICIENT = 0.02f;

        private float mMass;
        private float mBounciness; // 1.0 = super bouncy, 0.0 = zero bounce
        private float mStaticFrictionCoefficient;
        private float mDynamicFrictionCoefficient;


        protected override void Awake() {
            // TODO: get GameComponent here? (should be allocated at same time as this)
            // + need to link both ways... ~gameComponent.setMono(this)

            gameComponent.SetPhase((int)GameComponent.ComponentPhases.POST_PHYSICS);

        }


        public override void Reset() {
            mMass = DEFAULT_MASS;
            mBounciness = DEFAULT_BOUNCINESS;
            mStaticFrictionCoefficient = DEFAULT_STATIC_FRICTION_COEFFICIENT;
            mDynamicFrictionCoefficient = DEFAULT_DYNAMIC_FRICTION_COEFFICIENT;
        }

        // TODO: other methods (resolveCollision, etc.)

    }

}
*/


using UnityEngine;

namespace ReplicaEngine {

    public class ForceComponent : GameComponentBridge {

        private const float DEFAULT_MASS = 1.0f;
        private const float DEFAULT_BOUNCINESS = 0.1f;
        private const float DEFAULT_STATIC_FRICTION_COEFFICIENT = 0.05f;
        private const float DEFAULT_DYNAMIC_FRICTION_COEFFICIENT = 0.02f;

        private float mMass;
        private float mBounciness; // 1.0 = super bouncy, 0.0 = zero bounce
        private float mStaticFrictionCoefficient;
        private float mDynamicFrictionCoefficient;


        protected override void Awake() {
            Debug.Log("ForceComponent - Awake");
            base.Awake();


            SetPhase((int)GameComponent.ComponentPhases.POST_PHYSICS);

        }


        public override void Reset() {
            mMass = DEFAULT_MASS;
            mBounciness = DEFAULT_BOUNCINESS;
            mStaticFrictionCoefficient = DEFAULT_STATIC_FRICTION_COEFFICIENT;
            mDynamicFrictionCoefficient = DEFAULT_DYNAMIC_FRICTION_COEFFICIENT;
        }

        // TODO: other methods (resolveCollision, etc.)

    }

}

/*
namespace ReplicaEngine {

    public class ForceComponent : GameComponent {

//        public GameComponent gameComponent;


        private const float DEFAULT_MASS = 1.0f;
        private const float DEFAULT_BOUNCINESS = 0.1f;
        private const float DEFAULT_STATIC_FRICTION_COEFFICIENT = 0.05f;
        private const float DEFAULT_DYNAMIC_FRICTION_COEFFICIENT = 0.02f;

        private float mMass;
        private float mBounciness; // 1.0 = super bouncy, 0.0 = zero bounce
        private float mStaticFrictionCoefficient;
        private float mDynamicFrictionCoefficient;


        protected override void Awake() {
            // TODO: get GameComponent here? (should be allocated at same time as this)
            // + need to link both ways... ~gameComponent.setMono(this)

//            gameComponent.SetPhase((int)GameComponent.ComponentPhases.POST_PHYSICS);
            SetPhase((int)GameComponent.ComponentPhases.POST_PHYSICS);

        }


        public override void Reset() {
            mMass = DEFAULT_MASS;
            mBounciness = DEFAULT_BOUNCINESS;
            mStaticFrictionCoefficient = DEFAULT_STATIC_FRICTION_COEFFICIENT;
            mDynamicFrictionCoefficient = DEFAULT_DYNAMIC_FRICTION_COEFFICIENT;
        }

        // TODO: other methods (resolveCollision, etc.)

    }

}
*/
