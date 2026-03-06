using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop
{
    public List<GameModule> OnSale = new();
    public void Refresh()
    {
        OnSale = new();
        OnSale.Add(GameState.Run.Expedition.GetNextCommonModule());
        OnSale.Add(GameState.Run.Expedition.GetNextCommonModule());
        OnSale.Add(GameState.Run.Expedition.GetNextCommonModule());
    }
    public void Buy(GameModule module)
    {
        if (!OnSale.Contains(module)) return;

        if (GameState.Run.Expedition.Player.PlayerMoney >= module.price)
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
            GameState.Run.Expedition.Player.GiveMoney(module.price/2);
    }

    public void TriggerShop()
    {
        Refresh();
        UnityEngine.SceneManagement.SceneManager.LoadScene("basic_Shop");
    }
}