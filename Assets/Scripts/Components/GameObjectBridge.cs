using ReplicaEngine;
using UnityEngine;

public class GameObjectBridge : MonoBehaviour {

    private GameEntity gameEntity;


    //private void Awake() {
    private void Start() {

        // TODO: set as field?
        GameEntityFactory gameEntityFactory = BaseEntity.systemRegistry.gameEntityFactory;
        Debug.Log($"Factory (entity): {gameEntityFactory}");

        gameEntity = gameEntityFactory.AllocateEntity();

        // TODO: if not staticdata? ...
        // TODO: OK?
        // get all components of type GameComponentBridge and 'add them to the game object'
        GameComponentBridge[] gameComponentBridges = GetComponents<GameComponentBridge>();
        foreach (GameComponentBridge gameComponentBridge in gameComponentBridges) {
            gameEntity.Add(gameComponentBridge.GetGameComponent());
        }

        // TODO: here? how?
        //addStaticData

        // TODO: here? earlier?
        gameEntity.Reset();
    }

    // TODO: ...?

}
