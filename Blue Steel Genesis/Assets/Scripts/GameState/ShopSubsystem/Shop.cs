/*using System.Collections.Generic;
using System.Linq;

public class Shop
{
    List<GameModule> OnSale;
    void Refresh()
    {
        OnSale.Clear();
        OnSale.Add(GameState.Run.Expedition.GetNextModule());
        OnSale.Add(GameState.Run.Expedition.GetNextModule());
        OnSale.Add(GameState.Run.Expedition.GetNextModule());
    }
    void Buy(GameModule module)
    {
        //TODO
        if (!OnSale.Contains(module))
        {
            return;
        }
        if (GameState.Run.Expedition.Player.PlayerMoney >= module.price)
        {
            GameState.Run.Expedition.Player.LoseMoney(module.price);
            OnSale.Remove(module);
            GameState.Run.Expedition.Player.addModule(module);
        }
    }
    void Sell(GameModule module)
    {
        //≈сть ли у игрока этот модуль проверка
        GameState.Run.Expedition.Player.GaveMoney(module.price);
        //Remove modele?
    }

}*/