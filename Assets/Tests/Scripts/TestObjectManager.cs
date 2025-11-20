using ReplicaEngine;
using UnityEngine;

// TODO: add more tests!

public class TestEntityManager : MonoBehaviour {

    private void Awake() {

        EntityManager entityManager = new();

        PhasedEntity p = entityManager.FindByClass<PhasedEntity>();
        Debug.Log(p);

        PhasedEntity phased = new() {
            phase = 3
        };
        entityManager.Add(phased);

        entityManager.CommitUpdates();

        BaseEntity b = entityManager.FindByClass<BaseEntity>();
        Debug.Log(b);

        p = entityManager.FindByClass<PhasedEntity>();
        Debug.Log(p);
    }
}
