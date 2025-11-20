/* CollisionParamaters defines global parameters related to dynamic (entity vs entity) collisions. */

namespace ReplicaEngine {

    public sealed class CollisionParameters {
        // HitType describes the type of hit that a victim entity receives.  Victims may choose to 
        // react differently to the intersection depending on the hit type.
        // TODO: Make this a bit field so that entities can support multiple hit types.
        public sealed class HitType {
            public const int INVALID = -1;   // no type
            public const int HIT = 0;        // standard hit type
            public const int DEATH = 1;      // causes instant death
            public const int COLLECT = 2;    // causes collectable entities to be collected by the attacker
            public const int POSSESS = 3;    // causes possessable entities to become possessed
            public const int DEPRESS = 4;    // a hit indicating that the attacker is pressing into the victim
            public const int LAUNCH = 5;     // a hit indicating that the attacker will launch the victim
            public const int DIALOG = 6;     // indicates the attacker has touched a dialog entity
// TODO: test platforms stuff... (or remove)
//////// PLATFORM - BEGIN
//            public const int NBTYPES = 7;    // number of different hit types
//////// PLATFORM - MID
            public const int PLATFORM = 7;   // indicates the attacker has landed on victim's platform
            public const int NBTYPES = 8;    // number of different hit types
//////// PLATFORM - END
        }

    }

}
