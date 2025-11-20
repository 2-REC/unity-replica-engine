/* The system registry manages a collection of global singleton systems and entities.
 * However, it differs from the standard singleton pattern in a few important ways:
 * - The objects managed by the registry have an undefined lifetime.
 *   They may become invalid at any time, and they may not be valid at the beginning of the program.
 * - The only object that is always guaranteed to be valid is the SystemRegistry itself.
 * - There may be more than one SystemRegistry, and there may be more than one instance of any of
 *   the systems managed by SystemRegistry allocated at once.  For example, separate threads may
 *   maintain their own separate SystemRegistry instances.
 */

using System.Collections.Generic;

namespace ReplicaEngine {

    public class SystemRegistry : BaseEntity {

        /*
        public BufferLibrary bufferLibrary;
        public CameraSystem cameraSystem;
        public ChannelSystem channelSystem;
        public CollisionSystem collisionSystem;
        */
        public ContextParameters contextParameters;
        /*
        public CustomToastSystem customToastSystem;
        public DebugSystem debugSystem;
        public DrawableFactory drawableFactory;
        public EventRecorder eventRecorder;
        public GameEntityCollisionSystem gameEntityCollisionSystem;
        */
        public GameEntityFactory gameEntityFactory;
        /*
        public GameEntityManager gameEntityManager;
        public HitPointPool hitPointPool;
        public HotSpotSystem hotSpotSystem;
        public HudSystem hudSystem;
        public InputGameInterface inputGameInterface;
        public InputSystem inputSystem;
        public LevelBuilder levelBuilder;
        public LevelSystem levelSystem;
        public OpenGLSystem openGLSystem;
        public SoundSystem soundSystem;
        public TextureLibrary shortTermTextureLibrary;
        public TextureLibrary longTermTextureLibrary;
        */
        public TimeSystem timeSystem;
        /*
        public RenderSystem renderSystem;
        public VectorPool vectorPool;
        public VibrationSystem vibrationSystem;
        */

        // TODO: OK List? (instead of Java's ArrayList)
        private readonly List<BaseEntity> mItemsNeedingReset = new();

        public SystemRegistry() {}

        public void RegisterForReset(BaseEntity item) {
            bool contained = mItemsNeedingReset.Contains(item);

            if (!contained) {
                mItemsNeedingReset.Add(item);
            }
        }

       public override void Reset() {
            foreach (var item in mItemsNeedingReset) {
                item.Reset();
            }
        }

    }

}