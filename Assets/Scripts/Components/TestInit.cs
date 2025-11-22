
/*
Scripts executing at application start, when the first scene loads (before any 'Awake' script).
Requires a prefab named "GameManager" in the "Assets/Resources" folder.
 */

/*
using UnityEngine;

namespace ReplicaEngine {

    public class TestInit : MonoBehaviour {

        // TODO: not good, as loaded for EVERY scene!? (only want in game/level scenes)
        // +> might be usefull for other systems? (input, etc?)
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadMain() {
            Debug.Log("TestInit - LoadMain");

            GameObject main = Instantiate(Resources.Load("GameManager")) as GameObject;
            DontDestroyOnLoad(main);

        }

    }

}
*/
