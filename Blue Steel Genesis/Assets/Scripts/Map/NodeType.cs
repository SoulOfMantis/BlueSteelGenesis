using System;

namespace Map
{
    [Flags]
    public enum Node : short
    {
        DISABLED = 0,
        REGULAR_ENEMY = 0b_0000_0000_0000_0001,
        EVENT =         0b_0000_0000_0000_0010,
        SHOP =          0b_0000_0000_0000_0100,
        REST =          0b_0000_0000_0000_1000,
        ELITE_ENEMY =   0b_0000_0000_0001_0000,
        
        TREASURE =      0b_0000_0000_0010_0000,
        START =         0b_0000_0000_0100_0000,
        BOSS =          0b_0000_0000_1000_0000,
        BLACK_MARKET =  0b_0000_0001_0000_0000,

        RANDOMLY_GENERATABLE    = REGULAR_ENEMY | /*EVENT |*/ SHOP | /*REST | |*/ELITE_ENEMY,
        ALL_REGULAR             = REGULAR_ENEMY | EVENT | SHOP | REST | ELITE_ENEMY | TREASURE
    }
}
