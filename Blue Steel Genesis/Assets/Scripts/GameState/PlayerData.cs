using System;
using System.Collections.Generic;

public class PlayerData
{
    public int PlayerMoney
    {
        get => playerMoney;
        protected set => playerMoney = Math.Max(value, 0);
    }
    private int playerMoney = 0;
    public int currentHealth
    {
        get => current_health_;
        set => current_health_ = Math.Clamp(value, 0, maxHealth);
    }
    private int current_health_;


    public int maxHealth { get; set; }
    public int maxEnergy { get; set; }


    public uint money { get; set; }
    public uint materials { get; set; }


    public List<GameModule> modules = new();
}
