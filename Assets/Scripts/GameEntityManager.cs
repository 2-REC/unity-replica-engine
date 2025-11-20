/* A node in the game graph that manages the activation status of its children.
 * The GameEntityManager moves the entities it manages in and out of the active list
 * (that is, in and out of the game tree, causing them to be updated or ignored, respectively)
 * each frame based on the distance of that entity to the camera.
 * Entities may specify an "activation radius" to define an area around themselves so that
 * the position of the camera can be used to determine which entities should receive processing time
 * and which should be ignored.
 * Entities that do not move should have an activation radius that defines a sphere similar to
 * the size of the screen; they only need processing when they are visible.
 * Entities that move around will probably need larger regions so that they can leave the
 * visible area of the game world and not be immediately deactivated.
 */


using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;


namespace ReplicaEngine {

    public class GameEntityManager : EntityManager {

        const int MAX_GAME_ENTITIES = 384;

        readonly static HorizontalPositionComparator sGameEntityComparator = new HorizontalPositionComparator();

        float mMaxActivationRadius;
        FixedSizeArray<BaseEntity> mInactiveEntities;
        FixedSizeArray<GameEntity> mMarkedForDeathEntities;
        GameEntity mPlayer;
        bool mVisitingGraph;
        Vector2 mCameraFocus;


        public GameEntityManager(float maxActivationRadius) : base(MAX_GAME_ENTITIES) {
            mMaxActivationRadius = maxActivationRadius;

            mInactiveEntities = new FixedSizeArray<BaseEntity>(MAX_GAME_ENTITIES);
            mInactiveEntities.SetComparator(sGameEntityComparator);

            mMarkedForDeathEntities = new FixedSizeArray<GameEntity>(MAX_GAME_ENTITIES);
            mVisitingGraph = false;

            mCameraFocus = new Vector2();
        }

        /*
        protected override void Awake() {
            EntityManagerInit(MAX_GAME_ENTITIES);

            // TODO: handle case with mMaxActivationRadius == -1 (infinite radius, always active)
            // set default to -1
            //mMaxActivationRadius = maxActivationRadius;

            mInactiveEntities = new FixedSizeArray<BaseEntity>(MAX_GAME_ENTITIES);
            mInactiveEntities.SetComparator(sGameEntityComparator);

            mMarkedForDeathEntities = new FixedSizeArray<GameEntity>(MAX_GAME_ENTITIES);
            mVisitingGraph = false;

            mCameraFocus = new Vector2();
        }
        protected void GameEntityManagerInit(float maxActivationRadius) {
            mMaxActivationRadius = maxActivationRadius;
        }
        // TODO horrible... => allows to pass parameter
        public static GameEntityManager CreateComponent(GameObject targetObject, float maxActivationRadius) {
            GameEntityManager gameEntityManager = targetObject.AddComponent<GameEntityManager>();
            gameEntityManager.GameEntityManagerInit(maxActivationRadius);
            return gameEntityManager;
        }
        */

        public override void CommitUpdates() {
            base.CommitUpdates();

            GameEntityFactory factory = systemRegistry.gameEntityFactory;
            if (factory == null || mMarkedForDeathEntities.Count == 0)
                return;

            foreach (GameEntity gameEntity in mMarkedForDeathEntities) {
                factory.Destroy(gameEntity);
            }
            mMarkedForDeathEntities.Clear();
        }

        public override void CustomUpdate(float timeDelta, BaseEntity parent) {
            CommitUpdates();

            throw new Exception("!!!! TODO - GameEntityManager UPDATE !!!!");
            /*
            CameraSystem camera = sSystemRegistry.cameraSystem;

            mCameraFocus.set(camera.getFocusPositionX(), camera.getFocusPositionY());
            mVisitingGraph = true;
...            FixedSizeArray<BaseEntity> objects = getObjects();
            final int count = objects.getCount();

            if (count > 0) {
                final Object[] objectArray = objects.getArray();
                for (int i = count - 1; i >= 0; i--) {
                    GameObject gameObject = (GameObject)objectArray[i];
                    final float distance2 = mCameraFocus.distance2(gameObject.getPosition());
                    if (distance2 < (gameObject.activationRadius * gameObject.activationRadius)
                            || gameObject.activationRadius == -1) {
                        gameObject.CustomUpdate(timeDelta, this);
                    } else {
                        // Remove the object from the list.
                        // It's safe to just swap the current object with the last
                        // object because this list is being iterated backwards, so 
                        // the last object in the list has already been processed.
                        objects.swapWithLast(i);
                        objects.removeLast();
                        if (gameObject.destroyOnDeactivation) {
                            mMarkedForDeathObjects.add(gameObject);
                        } else {
                            mInactiveObjects.add((BaseObject)gameObject);
                        }
                    }
                }
            }

            mInactiveObjects.sort(false);
            final int inactiveCount = mInactiveObjects.getCount();
            if (inactiveCount > 0) {
                final Object[] inactiveArray = mInactiveObjects.getArray();
                for (int i = inactiveCount - 1; i >= 0; i--) {
                    GameObject gameObject = (GameObject)inactiveArray[i];

                    final Vector2 position = gameObject.getPosition();
                    final float distance2 = mCameraFocus.distance2(position);
                    final float xDistance = position.x - mCameraFocus.x;
                    if (distance2 < (gameObject.activationRadius * gameObject.activationRadius)
                            || gameObject.activationRadius == -1) {
                        gameObject.CustomUpdate(timeDelta, this);
                        mInactiveObjects.swapWithLast(i);
                        mInactiveObjects.removeLast();
                        objects.add(gameObject);
                        // TODO: is this test correct?
                        // => should also handle case with mMaxActivationRadius == -1 (infinite radius, always active)
                    } else if (xDistance < -mMaxActivationRadius) {
                        // We've passed the focus, we can stop processing now
                        break;
                    }
                }
            }
            mVisitingGraph = false;
            */
        }


        public override void Add(BaseEntity entity) {
            if (entity is GameEntity) {
                base.Add(entity);
            }
        }

        public override void Remove(BaseEntity entity) {
            base.Remove(entity);
            if (entity == mPlayer) {
                mPlayer = null;
            }
        }

        public void Destroy(GameEntity entity) {
            mMarkedForDeathEntities.Add(entity);
            Remove(entity);
        }

        public void DestroyAll() {
            // TODO: exception!
            //assert mVisitingGraph == false;
            if (mVisitingGraph)
                return;

            CommitUpdates();

            FixedSizeArray<BaseEntity> entities = GetEntities();
            foreach (BaseEntity entity in entities) {
                mMarkedForDeathEntities.Add((GameEntity)entity);
            }
            entities.Clear();

            foreach (BaseEntity inactiveEntity in mInactiveEntities) {
                mMarkedForDeathEntities.Add((GameEntity)inactiveEntity);
            }
            mInactiveEntities.Clear();

            mPlayer = null;
        }

        public void SetPlayer(GameEntity player) {
            mPlayer = player;
        }

        public GameEntity GetPlayer() {
            return mPlayer;
        }

        // private?
        class HorizontalPositionComparator : IComparer<BaseEntity> {
            public int Compare(BaseEntity object1, BaseEntity object2) {
                int result = 0;
                if (object1 == null && object2 != null) {
                    result = 1;
                } else if (object1 != null && object2 == null) {
                    result = -1;
                } else if (object1 != null && object2 != null) {
                    float delta = ((GameEntity)object1).GetPosition().x - ((GameEntity)object2).GetPosition().x;
                    if (delta < 0) {
                        result = -1;
                    } else if (delta > 0) {
                        result = 1;
                    }
                }
                return result;
            }
        }
    }

}
