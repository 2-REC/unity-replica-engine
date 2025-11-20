
// TODO: rename class!

namespace ReplicaEngine {
    public sealed class LerpTools {

        public static float Lerp(float start, float target, float duration, float timeSinceStart) {
            float value = start;
            if (timeSinceStart > 0.0f && timeSinceStart < duration) {
                float range = target - start;
                float percent = timeSinceStart / duration;
                value = start + (range * percent);
            } else if (timeSinceStart >= duration) {
                value = target;
            }
            return value;
        }

        public static float Ease(float start, float target, float duration, float timeSinceStart) {
            float value = start;
            if (timeSinceStart > 0.0f && timeSinceStart < duration) {
                float range = target - start;
                float percent = timeSinceStart / (duration / 2.0f);
                if (percent < 1.0f) {
                    value = start + ((range / 2.0f) * percent * percent * percent);
                } else {
                    float shiftedPercent = percent - 2.0f;
                    value = start + ((range / 2.0f) *
                            ((shiftedPercent * shiftedPercent * shiftedPercent) + 2.0f));
                }
            } else if (timeSinceStart >= duration) {
                value = target;
            }
            return value;
        }
    }

}
