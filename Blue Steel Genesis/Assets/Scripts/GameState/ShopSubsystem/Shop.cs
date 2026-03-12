using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RerollOptions
{
    Common,
    Boss
}

public class Shop
{
    uint biome;
    const uint rerollCostIncrease = 20;
    const uint bossRerollCostIncreaseMultiplier = 10;
    public uint RerollCost = 0;
    public Shop(uint biome)
    {
        this.biome = biome;
    }
    public List<GameModule> OnSale = new();
    public void Reroll(RerollOptions r)
    {
        if (GameState.Run.Expedition.Player.PlayerMoney >= RerollCost)
        {
            GameState.Run.Expedition.Player.LoseMoney((int)RerollCost);
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
        }
    }
    void CommonRefresh()
    {
        OnSale = new ();
        OnSale.Add(GameState.Run.Expedition.GetNextCommonModule(GameState.Run.Expedition.Player.modules));
        OnSale.Add(GameState.Run.Expedition.GetNextCommonModule(GameState.Run.Expedition.Player.modules.Union(OnSale).ToList()));
        OnSale.Add(GameState.Run.Expedition.GetNextCommonModule(GameState.Run.Expedition.Player.modules.Union(OnSale).ToList()));
    }
    void BossRefresh()
    {
        OnSale = new();
        OnSale.Add(GameState.Run.Expedition.GetNextBossModule(GameState.Run.Expedition.Player.modules));
        OnSale.Add(GameState.Run.Expedition.GetNextBossModule(GameState.Run.Expedition.Player.modules.Union(OnSale).ToList()));
        OnSale.Add(GameState.Run.Expedition.GetNextBossModule(GameState.Run.Expedition.Player.modules.Union(OnSale).ToList()));
    }
    public void Buy(GameModule module)
    {
        if (!OnSale.Contains(module)) return;
        if (GameState.Run.Expedition.Player.HasEnoughMoney(module.price))
        {
            GameState.Run.Expedition.Player.LoseMoney(module.price);
            OnSale.Remove(module);
            GameState.Run.Expedition.Player.AddModule(module);
            Debug.Log($"Player bought {module.Name} for {module.price} gold");
        }
    }
    public void Sell(GameModule module)
    {
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
        UnityEngine.SceneManagement.SceneManager.LoadScene($"basic_Shop_b{biome}");
    }
    public void TriggerBlackMarket()
    {
        RerollCost = 0;
        Reroll(RerollOptions.Boss);
        UnityEngine.SceneManagement.SceneManager.LoadScene($"basic_black_market_b{biome}");
    }

}