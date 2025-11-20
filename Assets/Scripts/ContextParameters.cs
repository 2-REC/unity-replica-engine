/* Contains global (but typically constant) parameters about the current operating context */

namespace ReplicaEngine {

    public class ContextParameters : BaseEntity {
        public int viewWidth;
        public int viewHeight;
// TODO: context!?
//        public Context context;
        public int gameWidth;
        public int gameHeight;
        public float viewScaleX;
        public float viewScaleY;
// TODO: check all are needed
        public bool supportsDrawTexture;
        public bool supportsVBOs;
        public int difficulty;

        // TODO: needed?
        public ContextParameters() {}

        public override void Reset() {}

    }

}
