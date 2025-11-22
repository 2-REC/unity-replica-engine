using UnityEngine;
using static ReplicaEngine.GameEntityFactory;

// TODO: should be created automatically in scene?
// => how? worth?

// TODO: CLEAN/ORGANISE!

namespace ReplicaEngine {

    // TODO: rename to Game|GameManager
    public class GameThread : MonoBehaviour {

        MainLoop mainLoop;

        private void Awake() {
            DontDestroyOnLoad(gameObject);

            // TODO: ADD ALL INITS!

            // TODO: init should not be done here... (?)
            /*
             view size: CHECK WHICH ONE!
             - Screen.width
             - mainCamera.pixelWidth
             - if (mainCamera.orthographic)
               {
                   float halfHeightWorld = mainCamera.orthographicSize;
                   float heightWorld = halfHeightWorld * 2;
                   float widthWorld = heightWorld * mainCamera.aspect;
               }
             */
            ContextParameters contextParameters = new() {
                viewWidth = 800, // TODO: CHANGE!!!!
                viewHeight = 600,
                //context ?
                gameWidth = 800,
                gameHeight = 600,
                viewScaleX = 1.0f,
                viewScaleY = 1.0f,
                // TODO: check all are needed
                /*
                supportsDrawTexture
                supportsVBOs
                difficulty
                */
            };
            BaseEntity.systemRegistry.contextParameters = contextParameters;

            // TODO: MORE INITS!


            mainLoop = new MainLoop();

            // TODO: here? (need to keep separate Game & GameThread?)
            //GameEntityManager gameManager = new(params.viewWidth * 2);
            GameEntityManager gameManager = new(800 * 2);
            BaseEntity.systemRegistry.gameEntityManager = gameManager;
            mainLoop.Add(gameManager);


            // TODO: make init method
            // TODO: set params! (MAX_GAME_OBJECTS, GameObjectType.OBJECT_COUNT.ordinal())
            //GameEntityFactory entityFactory = getGameObjectFactory();
            GameEntityFactory entityFactory = new(384, 128);

            BaseEntity.systemRegistry.gameEntityFactory = entityFactory;

            // TODO: DO SOMEWHERE ELSE!! (+where to get/set the numbers from?)
            // move to factory ctor?
            ComponentClass[] componentTypes = {
                new(typeof(MovementComponent), 1),
                new(typeof(ForceComponent), 1),
            };
            entityFactory.SetComponentClasses(componentTypes);



            // TODO: MORE INITS!


        }

        private void Update() {
            //Debug.Log($"GameThread - Update: {Time.deltaTime}");

            // TODO: deltaTime in correct unit?
            mainLoop.CustomUpdate(Time.deltaTime, null);
        }
    }

}
