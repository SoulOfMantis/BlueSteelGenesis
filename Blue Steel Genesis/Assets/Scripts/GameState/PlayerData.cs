using System;
using System.Collections.Generic;

public class PlayerData
{
    public int currentHealth
    {
        get => current_health_;
        set => current_health_ = Math.Clamp(value, 0, maxHealth);
    }
    private int current_health_;


    public int maxHealth { get; set; }
    public int maxEnergy { get; set; }

    public void GiveMoney(int value) => PlayerMoney += value;
    public void LoseMoney(int value) => PlayerMoney -= value;
    public void GiveMaterials(int value) => PlayerMaterials += value;
    public void LoseMaterials(int value) => PlayerMaterials -= value;

    public int PlayerMoney
    {
        get => playerMoney;
        protected set => playerMoney = Math.Max(value, 0);
    }
    private int playerMoney = 0;

    public int PlayerMaterials
    {
        get => playerMaterials;
        protected set => playerMaterials = Math.Max(value, 0);
    }
    private int playerMaterials = 0;

    public uint livesCount { get; set; }


    public List<GameModule> modules = new();
}
