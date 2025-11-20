
using System;

// TODO: see if keep or replace by C# methods (?)

namespace ReplicaEngine {

    public class Utils {
        private const float EPSILON = 0.0001f;

        public static bool Close(float a, float b) {
            return Close(a, b, EPSILON);
        }

        public static bool Close(float a, float b, float epsilon) {
            return Math.Abs(a - b) < epsilon;
        }

        public static int Sign(float a) {
            if (a >= 0.0f) {
                return 1;
            } else {
                return -1;
            }
        }

        public static int Clamp(int value, int min, int max) {
            int result = value;
            if (min == max) {
                if (value != min) {
                    result = min;
                }
            } else if (min < max) {
                if (value < min) {
                    result = min;
                } else if (value > max) {
                    result = max;
                }
            } else {
                result = Clamp(value, max, min);
            }

            return result;
        }


        public static int ByteArrayToInt(byte[] b) {
            if (b.Length != 4) {
                return 0;
            }

// TODO: CHECK!
            // Same as DataInputStream's 'readInt' method
            /*int i = (((b[0] & 0xff) << 24) | ((b[1] & 0xff) << 16) | ((b[2] & 0xff) << 8) 
                    | (b[3] & 0xff));*/

            // little endian
            int i = (((b[3] & 0xff) << 24) | ((b[2] & 0xff) << 16) | ((b[1] & 0xff) << 8)
                    | (b[0] & 0xff));

            return i;
        }

        public static float ByteArrayToFloat(byte[] b) {

// TODO: CHECK!
            // intBitsToFloat() converts bits as follows:
            /*
            int s = ((i >> 31) == 0) ? 1 : -1;
            int e = ((i >> 23) & 0xff);
            int m = (e == 0) ? (i & 0x7fffff) << 1 : (i & 0x7fffff) | 0x800000;
            */

// TODO: OK?
            return BitConverter.Int32BitsToSingle(ByteArrayToInt(b));
        }

        public static float FramesToTime(int framesPerSecond, int frameCount) {
            return (1.0f / framesPerSecond) * frameCount;
        }

    }

}