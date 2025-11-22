using ReplicaEngine;
using UnityEngine;

// TODO: RENAME!!!!
public abstract class GameComponentBridge : MonoBehaviour {

    private GameComponent gameComponent;


    protected virtual void Awake() {
        Debug.Log("GameComponentBridge - Awake");

        // TODO: get GameComponent here? (should be allocated at same time as this)
        // + need to link both ways...

        // TODO: set as field?
        GameEntityFactory gameEntityFactory = BaseEntity.systemRegistry.gameEntityFactory;
        Debug.Log($"Factory (component): {gameEntityFactory}");

        // TODO: GetType?
        Debug.Log($"COMPONENT TYPE: {this.GetType()}");
        gameComponent = gameEntityFactory.AllocateComponent(this.GetType());

        gameComponent.SetComponent(this);
        // ... more?

        gameComponent.Reset();
    }

    private void OnDestroy() {
        Debug.Log("GameComponentBridge - OnDestroy");

    }


    // TODO: rename to 'SetComponentPhase'?
    public void SetPhase(int phaseValue) {
        gameComponent.SetPhase(phaseValue);
    }

    public abstract void Reset();

    // TODO: ok?
    public virtual void CustomUpdate(float timeDelta, BaseEntity parent) {
        Debug.Log("GameComponentBridge - CustomUpdate");
    }

    public GameComponent GetGameComponent() {
        return gameComponent;
    }

}
