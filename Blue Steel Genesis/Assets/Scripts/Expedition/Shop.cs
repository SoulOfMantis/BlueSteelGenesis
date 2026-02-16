using System.Collections.Generic;
using System.Linq;

public class Shop
{
    List<GameModule> OnSale;
    PlayerCharacter player;
    void Refresh()
    {
        //TODO
        OnSale.Clear();
        //What module add
        return;
    }
    void Buy(GameModule module)
    {
        //TODO
        if (!OnSale.Contains(module))
        {
            return;
        }
        if (player.playerMoney >= module.price)
        {
            player.LoseMoney(module.Price);
            OnSale.Remove(module);
            player.addModule(module);
        }
    }
    void Sell(GameModule module)
    {
        //≈сть ли у игрока этот модуль проверка
        player.GaveMoney(module.Price);
        //Remove modele?
    }

}