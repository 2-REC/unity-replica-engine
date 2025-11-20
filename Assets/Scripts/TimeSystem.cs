/* Maintains a canonical time step, in seconds, for the entire game engine.
 * This time step represents real changes in time but is only updated once per frame.
 */

// TODO: time distortion effects could go here, or they could go into a special entity manager.

namespace ReplicaEngine {

    public class TimeSystem : BaseEntity {
        private const float EASE_DURATION = 0.5f;

        float mGameTime;
        float mRealTime;
        float mFreezeDelay;
        float mGameFrameDelta;
        float mRealFrameDelta;

        float mTargetScale;
        float mScaleDuration;
        float mScaleStartTime;
        bool mEaseScale;


        public TimeSystem() {
            Reset();
        }

        public override void Reset() {
            mGameTime = 0.0f;
            mRealTime = 0.0f;
            mFreezeDelay = 0.0f;
            mGameFrameDelta = 0.0f;
            mRealFrameDelta = 0.0f;

            mTargetScale = 1.0f;
            mScaleDuration = 0.0f;
            mScaleStartTime = 0.0f;
            mEaseScale = false;
        }

        public override void CustomUpdate(float timeDelta, BaseEntity parent) {
            mRealTime += timeDelta;
            mRealFrameDelta = timeDelta;

            if (mFreezeDelay > 0.0f) {
                mFreezeDelay -= timeDelta;
                mGameFrameDelta = 0.0f;
            } else {
                float scale = 1.0f;
                if (mScaleStartTime > 0.0f) {
                    float scaleTime = mRealTime - mScaleStartTime;
                    if (scaleTime > mScaleDuration) {
                        mScaleStartTime = 0;
                    } else {
                        if (mEaseScale) {
                            if (scaleTime <= EASE_DURATION) {
                                // ease in
                                scale = LerpTools.Ease(1.0f, mTargetScale, EASE_DURATION, scaleTime);
                            } else if (mScaleDuration - scaleTime < EASE_DURATION) {
                                // ease out
                                float easeOutTime = EASE_DURATION - (mScaleDuration - scaleTime);
                                scale = LerpTools.Ease(mTargetScale, 1.0f, EASE_DURATION, easeOutTime);
                            } else {
                                scale = mTargetScale;
                            }
                        } else {
                            scale = mTargetScale;
                        }
                    }
                }

                mGameTime += (timeDelta * scale);
                mGameFrameDelta = (timeDelta * scale);
            }


        }

        public float GetGameTime() {
            return mGameTime;
        }

        public float GetRealTime() {
            return mRealTime;
        }

        public float GetFrameDelta() {
            return mGameFrameDelta;
        }

        public float GetRealTimeFrameDelta() {
            return mRealFrameDelta;
        }

        public void Freeze(float seconds) {
            mFreezeDelay = seconds;
        }

        public void ApplyScale(float scaleFactor, float duration, bool ease) {
            mTargetScale = scaleFactor;
            mScaleDuration = duration;
            mEaseScale = ease;
            if (mScaleStartTime <= 0.0f) {
                mScaleStartTime = mRealTime;
            }
        }

        //////// SPEEDUP - MID
// TODO: comment?
        public void ExtendScale(float duration) {
            mScaleDuration = duration;
            mScaleStartTime = mRealTime;
        }
        //////// SPEEDUP - END

    }

}
