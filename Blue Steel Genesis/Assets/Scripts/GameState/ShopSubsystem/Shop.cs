using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RerollOptions
{
    Common,
    Boss
}

[Serializable]
public class Shop
{
    [SerializeField]
    uint biome;
    const uint rerollCostIncrease = 20;
    const uint bossRerollCostIncreaseMultiplier = 10;
    [SerializeField]
    public uint RerollCost = 0;
    public Shop(uint biome)
    {
        this.biome = biome;
    }
    public List<GameModule> OnSale = new();
    public bool Reroll(RerollOptions r)
    {
        if (GameState.Run.Expedition.Player.HasEnoughMoney(RerollCost))
        {
            GameState.Run.Expedition.Player.LoseMoney(RerollCost);
            switch (r)
            {
                case RerollOptions.Common:
                    CommonRefresh();
                    RerollCost += rerollCostIncrease;
                    break;
                case RerollOptions.Boss:
                    BossRefresh();
                    RerollCost += rerollCostIncrease * bossRerollCostIncreaseMultiplier;
                    break;
                default:
                    break;
            }
            return true;
        }
        return false;
    }
    void CommonRefresh()
    {
        OnSale = new ();
        OnSale.Add(GameState.Run.Expedition.ModuleGen.GetNextCommonModule(GameState.Run.Expedition.Player.modules));
        OnSale.Add(GameState.Run.Expedition.ModuleGen.GetNextCommonModule(GameState.Run.Expedition.Player.modules.Union(OnSale)));
        OnSale.Add(GameState.Run.Expedition.ModuleGen.GetNextCommonModule(GameState.Run.Expedition.Player.modules.Union(OnSale)));
    }
    void BossRefresh()
    {
        OnSale = new();
        OnSale.Add(GameState.Run.Expedition.ModuleGen.GetNextBossModule(GameState.Run.Expedition.Player.modules));
        OnSale.Add(GameState.Run.Expedition.ModuleGen.GetNextBossModule(GameState.Run.Expedition.Player.modules.Union(OnSale)));
        OnSale.Add(GameState.Run.Expedition.ModuleGen.GetNextBossModule(GameState.Run.Expedition.Player.modules.Union(OnSale)));
    }
    public bool Buy(GameModule module)
    {
        if (!OnSale.Contains(module)) return false;
        switch (ModuleGenerator.isBoss(module))
        {
            case true:
                if (GameState.Run.Expedition.Player.HasGoldenTickets())
                {
                    GameState.Run.Expedition.Player.SpendGoldenTicket();
                    OnSale.Remove(module);
                    GameState.Run.Expedition.Player.AddModule(module);
                    Debug.Log($"Player bought {module.Name} for a golden ticket");
                    return true;
                }
                break;

            case false:
                if (GameState.Run.Expedition.Player.HasEnoughMoney(module.price))
                {
                    GameState.Run.Expedition.Player.LoseMoney(module.price);
                    OnSale.Remove(module);
                    GameState.Run.Expedition.Player.AddModule(module);
                    Debug.Log($"Player bought {module.Name} for {module.price} gold");
                    return true;
                }
                break;
        }
        return false;
    }
    public void Sell(GameModule module)
    {
        if (ModuleGenerator.isBoss(module))
        {
            Debug.Log("Player tried to sell a boss module");
            return;
        }
        if (GameState.Run.Expedition.Player.RemoveModule(module))
        {
            GameState.Run.Expedition.Player.GiveMoney(module.price/2);
            Debug.Log($"Player sold {module.Name} and got {module.price/2} gold");
        }
    }

    public void TriggerShop()
    {
        RerollCost = 0;
        Reroll(RerollOptions.Common);
        UnityEngine.SceneManagement.SceneManager.LoadScene($"basic_shop_b{biome}");
    }
    public void TriggerBlackMarket()
    {
        RerollCost = 0;
        Reroll(RerollOptions.Boss);
        UnityEngine.SceneManagement.SceneManager.LoadScene($"basic_black_market_b{biome}");
    }

}