using System.Collections.Generic;
using System.Linq;

public class Shop
{
    List<GameModule> OnSale;
    PlayerCharacter Player;
    int PlayerMoney;
    /*public Shop()
    {

    }*/
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
            // What in this case?
        }
        if (PlayerMoney >= module.Price)
        {
            PlayerMoney -= module.Price;
            OnSale.Remove(module);
            Player.addModule(module);
        }
        return;
    }
    void Sell(GameModule module)
    {
        PlayerMoney += module.Price;
        //Remove modele?
        return;
    }

}