using System;

namespace Map
{
    [Flags]
    public enum Node : byte
    {
        DISABLED = 0,
        REGULAR_ENEMY = 0b_0000_0001,
        EVENT =         0b_0000_0010,
        SHOP =          0b_0000_0100,
        TREASURE =      0b_0000_1000,
        REST =          0b_0001_0000,
        ELITE_ENEMY =   0b_0010_0000,

        START =         0b_0100_0000,
        BOSS =          0b_1000_0000,

        ALL_REGULAR = REGULAR_ENEMY | EVENT | SHOP | TREASURE | REST | ELITE_ENEMY
    }
}
