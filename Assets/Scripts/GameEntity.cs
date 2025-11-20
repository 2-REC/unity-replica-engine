/* GameEntity defines any entity that resides in the game world (character, background, special effect, enemy, etc).
 * It is a collection of GameComponents which implement its behavior; a GameEntity itself has no intrinsic behavior.
 * A GameEntity is also a "bag of data" that components can use to share state
 * (direct component-to-component communication is discouraged).
 */

using UnityEngine;
using static ReplicaEngine.CollisionParameters;

namespace ReplicaEngine {

    public class GameEntity : PhasedEntityManager {

        const float COLLISION_SURFACE_DECAY_TIME = 0.3f;
        const int DEFAULT_LIFE = 1;
        //////// STRENGTH - MID
        const int DEFAULT_STRENGTH = 1;
        //////// STRENGTH - END

        public enum Team {
            NONE,
            PLAYER,
            ENEMY
        }

        public enum ActionType {
            INVALID,
            IDLE,
            MOVE,
            ATTACK,
            HIT_REACT,
            DEATH,
            //////// WIN - BEGIN
            WIN,
            //////// WIN - END
            HIDE,
            FROZEN
        }

        public Team team;

        public bool positionLocked;

        public float activationRadius;
        public bool destroyOnDeactivation;

        public int life;
        //////// STRENGTH - MID
        public int strength;
        //////// STRENGTH - END

        public int lastReceivedHitType;

        public Vector2 facingDirection;
        public float width;
        public float height;

        private MonoBehaviour gameObject;

        // TODO: check impact on performance
        //Vector2 mPosition;
        Vector2 mPosition {
            get => new(gameObject.transform.position.x, gameObject.transform.position.y);
            set => gameObject.transform.position = new Vector3(value.x, value.y, gameObject.transform.position.z);
        }
        Vector2 mVelocity;
        Vector2 mTargetVelocity;
        Vector2 mAcceleration;
        Vector2 mImpulse;

        Vector2 mBackgroundCollisionNormal;

        float mLastTouchedFloorTime;
        float mLastTouchedCeilingTime;
        float mLastTouchedLeftWallTime;
        float mLastTouchedRightWallTime;

        ActionType mCurrentAction;


        // TODO: make sure it calls base ctor
        public GameEntity() {
            // useless as done in Reset
            ////mPosition = new Vector2();
            //mVelocity = new Vector2();
            //mTargetVelocity = new Vector2();
            //mAcceleration = new Vector2();
            //mImpulse = new Vector2();
            //mBackgroundCollisionNormal = new Vector2();

// TODO: is this one correct? or in Reset? (reset anyway)
            facingDirection = new Vector2(1, 0);
            Reset();
        }

        /*
        protected override void Awake() {
            base.Awake();
            // TODO: is this one correct? or in Reset? (reset anyway)
            facingDirection = new Vector2(1, 0);
            Reset();
        }
        */
        /*
        protected override void Init() {
            base.Init();
            // TODO: is this one correct? or in Reset? (reset anyway)
            facingDirection = new Vector2(1, 0);
            Reset();
        }
        // TODO: 'new' ok? (if not, base can't be static)
        public static new GameEntity Create(GameObject targetObject) {
            GameEntity gameEntity = targetObject.AddComponent<GameEntity>();
            gameEntity.Init();
            return gameEntity;
        }
        */

        public override void Reset() {
            RemoveAll();
            CommitUpdates();

            //mPosition = Vector2.zero;
            mVelocity = Vector2.zero;
            mTargetVelocity = Vector2.zero;
            mAcceleration = Vector2.zero;
            mImpulse = Vector2.zero;
            mBackgroundCollisionNormal = Vector2.zero;
            // TODO: error ? (was marked as error, to check!)
            facingDirection.Set(1.0f, 1.0f);

            mCurrentAction = ActionType.INVALID;
            positionLocked = false;
            activationRadius = 0;
            destroyOnDeactivation = false;
            life = DEFAULT_LIFE;
            //////// STRENGTH - MID
            strength = DEFAULT_STRENGTH;
            //////// STRENGTH - END
            team = Team.NONE;
            width = 0.0f;
            height = 0.0f;

            lastReceivedHitType = HitType.INVALID;
        }

        public bool TouchingGround() {
            TimeSystem time = systemRegistry.timeSystem;
            float gameTime = time.GetGameTime();
            bool touching = gameTime > 0.1f &&
                Utils.Close(mLastTouchedFloorTime, gameTime, COLLISION_SURFACE_DECAY_TIME);
            return touching;
        }

        public bool TouchingCeiling() {
            TimeSystem time = systemRegistry.timeSystem;
            float gameTime = time.GetGameTime();
            bool touching = gameTime > 0.1f &&
                Utils.Close(mLastTouchedCeilingTime, gameTime, COLLISION_SURFACE_DECAY_TIME);
            return touching;
        }

        public bool TouchingLeftWall() {
            TimeSystem time = systemRegistry.timeSystem;
            float gameTime = time.GetGameTime();
            bool touching = gameTime > 0.1f &&
                Utils.Close(mLastTouchedLeftWallTime, gameTime, COLLISION_SURFACE_DECAY_TIME);
            return touching;
        }

        public bool TouchingRightWall() {
            TimeSystem time = systemRegistry.timeSystem;
            float gameTime = time.GetGameTime();
            bool touching = gameTime > 0.1f &&
                Utils.Close(mLastTouchedRightWallTime, gameTime, COLLISION_SURFACE_DECAY_TIME);
            return touching;
        }

// TODO: replace all by getters/setters
// TODO: 'swap' position and centeredPosition?
        public Vector2 GetPosition() {
            return mPosition;
        }

        public void SetPosition(Vector2 position) {
            mPosition.Set(position.x, position.y);
        }

        public float GetCenteredPositionX() {
            return mPosition.x + (width / 2.0f);
        }

        public float GetCenteredPositionY() {
            return mPosition.y + (height / 2.0f);
        }

        public Vector2 GetVelocity() {
            return mVelocity;
        }

        public void SetVelocity(Vector2 velocity) {
            mVelocity.Set(velocity.x, velocity.y);
        }

        public Vector2 GetTargetVelocity() {
            return mTargetVelocity;
        }

        public void SetTargetVelocity(Vector2 targetVelocity) {
            mTargetVelocity.Set(targetVelocity.x, targetVelocity.y);
        }

        public Vector2 getAcceleration() {
            return mAcceleration;
        }

        public void SetAcceleration(Vector2 acceleration) {
            mAcceleration.Set(acceleration.x, acceleration.y);
        }

        public Vector2 GetImpulse() {
            return mImpulse;
        }

        public void SetImpulse(Vector2 impulse) {
            mImpulse.Set(impulse.x, impulse.y);
        }

        public Vector2 GetBackgroundCollisionNormal() {
            return mBackgroundCollisionNormal;
        }

        public void SetBackgroundCollisionNormal(Vector2 normal) {
            mBackgroundCollisionNormal.Set(normal.x, normal.y);
        }

        public float GetLastTouchedFloorTime() {
            return mLastTouchedFloorTime;
        }

        public void SetLastTouchedFloorTime(float lastTouchedFloorTime) {
            mLastTouchedFloorTime = lastTouchedFloorTime;
        }

        public float GetLastTouchedCeilingTime() {
            return mLastTouchedCeilingTime;
        }

        public void SetLastTouchedCeilingTime(float lastTouchedCeilingTime) {
            mLastTouchedCeilingTime = lastTouchedCeilingTime;
        }

        public float GetLastTouchedLeftWallTime() {
            return mLastTouchedLeftWallTime;
        }

        public void SetLastTouchedLeftWallTime(float lastTouchedLeftWallTime) {
            mLastTouchedLeftWallTime = lastTouchedLeftWallTime;
        }

        public float GetLastTouchedRightWallTime() {
            return mLastTouchedRightWallTime;
        }

        public void SetLastTouchedRightWallTime(float lastTouchedRightWallTime) {
            mLastTouchedRightWallTime = lastTouchedRightWallTime;
        }

        public ActionType GetCurrentAction() {
            return mCurrentAction;
        }

        public void SetCurrentAction(ActionType type) {
            mCurrentAction = type;
        }

        //////// SPEEDUP - MID
// TODO: keep? (TO TEST!)
        public float GetCollisionSurfaceDecayTime() {
            return COLLISION_SURFACE_DECAY_TIME;
        }
        //////// SPEEDUP - END

    }

}
