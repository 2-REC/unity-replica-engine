/* A class for generating game entities at runtime.
 * This should really be replaced with something that is data-driven, but it is hard to do data
 * parsing quickly at runtime.
 * For the moment this class is full of large functions that just patch pointers between entities,
 * but in the future those functions should either be:
 * a) generated from data at compile time,
 * b) described by data at runtime.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ReplicaEngine {

    public abstract class GameEntityFactory : BaseEntity {

        readonly static ComponentPoolComparator sComponentPoolComparator = new ComponentPoolComparator();

        protected FixedSizeArray<FixedSizeArray<BaseEntity>> mStaticData;
        private FixedSizeArray<GameComponentPool> mComponentPools;
        private GameComponentPool mPoolSearchDummy;
        protected GameEntityPool mGameEntityPool;

        protected float mTightActivationRadius;
        protected float mNormalActivationRadius;
        protected float mWideActivationRadius;
        protected float mAlwaysActive;


        protected class ComponentClass {
            public Type type;
            public int poolSize;

            public ComponentClass(Type classType, int size) {
                type = classType;
                poolSize = size;
            }
        }

        // TODO: need to call base ctor?
        public GameEntityFactory(int maxNbEntities, int nbEntityTypes) {
            mGameEntityPool = new GameEntityPool(maxNbEntities);

            int entityTypesCount = nbEntityTypes;
            mStaticData = new FixedSizeArray<FixedSizeArray<BaseEntity>>(entityTypesCount);
            // TODO: OK? needed?
            for (int x = 0; x < entityTypesCount; x++) {
                mStaticData.Add(null);
            }

            ContextParameters context = systemRegistry.contextParameters;
            float halfHeight2 = (context.gameHeight * 0.5f) * (context.gameHeight * 0.5f);
            float halfWidth2 = (context.gameWidth * 0.5f) * (context.gameWidth * 0.5f);
            float screenSizeRadius = (float)Math.Sqrt(halfHeight2 + halfWidth2);
            mTightActivationRadius = screenSizeRadius * 0.75f;
            mNormalActivationRadius = screenSizeRadius * 1.25f;
            mWideActivationRadius = screenSizeRadius * 2.0f;
            mAlwaysActive = -1.0f;
        }

        protected void SetComponentClasses(ComponentClass[] componentTypes) {
            mComponentPools = new FixedSizeArray<GameComponentPool>(componentTypes.Length, sComponentPoolComparator);
            foreach (ComponentClass componentClass in componentTypes) {
                mComponentPools.Add(new GameComponentPool(componentClass.type, componentClass.poolSize));
            }
            mComponentPools.Sort(true);

            // TODO: typeof ok?
            mPoolSearchDummy = new GameComponentPool(typeof(object), 1);
        }

        public override void Reset() {}

        protected GameComponentPool GetComponentPool(Type componentType) {
            GameComponentPool pool = null;
            mPoolSearchDummy.entityClass = componentType;
            int index = mComponentPools.Find(mPoolSearchDummy, false);
            if (index != -1) {
                pool = mComponentPools[index];
            }
            return pool;
        }

        protected GameComponent AllocateComponent(Type componentType) {
            GameComponentPool pool = GetComponentPool(componentType);

            // TODO: exception?
            //assert pool != null;

            GameComponent component = null;
            if (pool != null) {
                component = pool.Allocate();
            }
            return component;
        }

        protected void ReleaseComponent(GameComponent component) {
            GameComponentPool pool = GetComponentPool(component.GetType());

            // TODO: exception?
            //assert pool != null;

            if (pool != null) {
                component.Reset();
                component.shared = false;
                pool.Release(component);
            }
        }

        protected bool ComponentAvailable(Type componentType, int count) {
            bool canAllocate = false;
            GameComponentPool pool = GetComponentPool(componentType);

            // TODO: exception?
            //assert pool != null;

            if (pool != null) {
                //canAllocate = pool.GetAllocatedCount() + count < pool.GetSize();
                // TODO: check ok
                canAllocate = pool.CountInactive >= count;
            }
            return canAllocate;
        }

        /*
        public void Destroy(GameObject object) {
            object.commitUpdates();
            final int componentCount = object.getCount();
            for (int x = 0; x < componentCount; x++) {
                GameComponent component = (GameComponent)object.get(x);
                if (!component.shared) {
                    releaseComponent(component);
                }
            }
            object.removeAll();
            object.commitUpdates();
            mGameObjectPool.release(object);
        }
        */
        // TODO: GameObject|GameEntity? => complicated with GameObject+GameEntity mix!
        // => see how to properly handle (release both MonoBehaviour and GameEntity components?)
        // MATCH/SYNC 'mGameObjectPool' WITH 'MonoBehaviour' POOL !?!
        //public void Destroy(GameObject gameObject) {
        public void Destroy(GameEntity gameObject) {
            throw new NotImplementedException("!!!! TODO - Destroy !!!!");
        }

        private FixedSizeArray<BaseEntity> GetStaticData(int index) {
            return mStaticData[index];
        }

        protected void SetStaticData(int index, FixedSizeArray<BaseEntity> data) {
            // TODO: add check!
            //assert mStaticData.get(index) == null;

            int staticDataCount = data.GetCount();

            foreach (BaseEntity entity in data) {
                if (entity is GameComponent component) {
                    component.shared = true;
                }
            }

            mStaticData[index] = data;
        }

        /*
        protected void AddStaticData(int index, GameObject object, SpriteComponent sprite) {
            FixedSizeArray<BaseObject> staticData = getStaticData(index);
            assert staticData != null;

            if (staticData != null) {
                final int staticDataCount = staticData.getCount();

                for (int x = 0; x < staticDataCount; x++) {
                    BaseObject entry = staticData.get(x);
                    if (entry instanceof GameComponent && object != null) {
                        object.add((GameComponent)entry);
                    } else if (entry instanceof SpriteAnimation && sprite != null) {
                        sprite.addAnimation((SpriteAnimation)entry);
                    }
                }
            }
        }
        */
        // TODO: allocate MonoBehaviour components here, set their associated GameEntity component, and add to GameObject (?)
        // MATCH/SYNC 'mGameObjectPool' WITH 'MonoBehaviour' POOL !?!
        /*
        protected void AddStaticData(int index, GameObject gamObject, SpriteComponent sprite) {
            throw new NotImplementedException("!!!! TODO - AddStaticData !!!!");
        }
        */

        // TODO: make sure the MonoBehaviour components are also released (but not here...)
        // MATCH/SYNC 'mGameObjectPool' WITH 'MonoBehaviour' POOL !?!
        public void ClearStaticData() {
            for (int i = 0; i<mStaticData.Count; ++i) {
                FixedSizeArray<BaseEntity> entityStaticData = mStaticData[i];
                if (entityStaticData == null)
                    continue;

                foreach (BaseEntity entity in entityStaticData) {
                    if (entity != null && entity is GameComponent component) {
                        ReleaseComponent(component);
                    }
                }
                entityStaticData.Clear();
                mStaticData[i] = null;
            }
        }

        public void SanityCheckPools() {
            int outstandingObjects = mGameEntityPool.CountActive;
            if (outstandingObjects != 0) {
                // TODO: exception type?
                throw new Exception($"Sanity Check: Outstanding game entity allocations! ({outstandingObjects})");
            }

            foreach (GameComponentPool componentPool in mComponentPools) {
                int outstandingComponents = componentPool.CountActive;
                if (outstandingComponents != 0) {
                    Debug.Log($"Sanity Check: Outstanding {componentPool.entityClass.Name} allocations! ({outstandingComponents})");
                    // TODO: no exception?
                    // ...
                }

            }
        }


        /*
        public void SpawnFromWorld(TiledWorld world, int tileWidth, int tileHeight) {
            // Walk the world and spawn objects based on tile indexes
            final float worldHeight = world.getHeight() * tileHeight;
            GameObjectManager manager = sSystemRegistry.gameObjectManager;
            if (manager != null) {
                for (int y = 0; y < world.getHeight(); y++) {
                    for (int x = 0; x < world.getWidth(); x++) {
                        int index = world.getTile(x, y);
                        if (index != -1) {
                            final float worldX = x * tileWidth;
                            final float worldY = worldHeight - ((y + 1) * tileHeight);
                            GameObject object = spawnFromIndex(index, worldX, worldY, false);
                            if (object != null) {
                                if (object.height < tileHeight) {
                                    // make sure small objects are vertically centered in their
                                    // tile.
                                    object.getPosition().y += (tileHeight - object.height) / 2.0f;
                                }
                                if (object.width < tileWidth) {
                                    object.getPosition().x += (tileWidth - object.width) / 2.0f;
                                } else if (object.width > tileWidth) {
                                    object.getPosition().x -= (object.width - tileWidth) / 2.0f;
                                }
                                manager.add(object);
                                if (isPlayer(index)) {
                                    manager.setPlayer(object);
                                }
                            }
                        }
                    }
                }
            }
        }
        */
        // !!!! TODO !!!!
        /*
        public void SpawnFromWorld(TiledWorld world, int tileWidth, int tileHeight) {
        }
        */

        private class ComponentPoolComparator : IComparer<GameComponentPool> {
            public int Compare(GameComponentPool object1, GameComponentPool object2) {
                int result = 0;
                if (object1 == null && object2 != null) {
                    result = 1;
                } else if (object1 != null && object2 == null) {
                    result = -1;
                } else if (object1 != null && object2 != null) {
                    // TODO: GetHashCode ok?
                    result = object1.entityClass.GetHashCode() - object2.entityClass.GetHashCode();
                }
                return result;
            }
        }

        // TODO: private?
        public class GameEntityPool : TEntityPool<GameEntity> {

            // TODO: make sure base ctor is called
            public GameEntityPool() {}

            public GameEntityPool(int size) : base(size) {}

            // TODO: 'object' ok? (if not, try with 'GameEntity')
            protected override object CreateItem() {
                return new GameEntity();
            }

            /*
            public override void Release(object entry) {
                ((GameEntity)entry).Reset();
                base.Release(entry);
            }
            */
        // TODO: 'object' ok? (if not, try with 'GameEntity')
        protected override void OnRelease(object entry) {
                ((GameEntity)entry).Reset();
            }

        }

    }

}
