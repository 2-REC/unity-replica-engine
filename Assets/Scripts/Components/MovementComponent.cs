using ReplicaEngine;
using UnityEngine;

public class MovementComponent : GameComponentBridge {

    protected override void Awake() {
        Debug.Log("MovementComponent - Awake");
        base.Awake();

        SetPhase((int)GameComponent.ComponentPhases.MOVEMENT);

    }

    public override void Reset() { }

    public override void CustomUpdate(float timeDelta, BaseEntity parent) {
        // !!!! TODO !!!!
        Debug.Log("MovementComponent - CustomUpdate");
    }

}

/*
using UnityEngine;


[Serializable]
public class MovementComponent : GameComponent {

    public string testString;

    public MovementComponent() {
        Debug.Log("CTOR!!!!");
    }

    public override void Reset() { }

    public override void CustomUpdate(float timeDelta, BaseEntity parent) {
        // !!!! TODO !!!!
    }

}
*/
