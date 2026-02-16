using System.Collections.Generic;
using System.Linq;
using System;

public class Expedition
{
    static Expedition instance;
    static void CreateInstance()
    {
        instance = new Expedition();
    }
    static GameModule DrawNextModule()
    {
        instance.moduleGeneration.DrawNextModule();
    }
    internal class ModuleGeneration
    {
        private Random gen;
        public ModuleGeneration(int Seed)
        {
            seed = Seed;
            gen = new Random(seed);
        }
        public GameModule DrawNextModule()
        {
            //TODO
            return new GameModule();
        }
    }

    internal class Shop
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
            //Wait Remove modele
        }
    }

    ModuleGeneration moduleGeneration;
}