using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Expedition: MonoBehaviour
{
    
    static Expedition instance;
    static void CreateInstance()
    {
        instance = new Expedition();
    }
    void Start()
    {
        CreateInstance();
    }
    static GameModule DrawNextModule()
    {
        return instance.moduleGeneration.DrawNextModule();
    }
    internal class ModuleGeneration
    {
        private System.Random gen;
        public ModuleGeneration(int seed)
        {
            gen = new System.Random(seed);
        }
        public GameModule DrawNextModule()
        {
            //TODO
            return null;
        }
    }

    internal class Shop
    {
        List<GameModule> OnSale;
        PlayerCharacter player;
        OnSaleModuleName OnSaleName;
       public void Refresh()
        {
            OnSale.Clear();
            OnSale.Add(DrawNextModule());
            OnSale.Add(DrawNextModule());
            OnSale.Add(DrawNextModule());
            return;
        }
        
        void Buy(GameModule module)
        {
            if (!OnSale.Contains(module))
            {
                return;
            }
            if (player.playerMoney >= module.price)
            {
                player.LoseMoney(module.price);
                OnSale.Remove(module);
                player.addModule(module);
            }
        }
        void Sell(GameModule module)
        {
            //Есть ли у игрока этот модуль проверь
            player.GaveMoney(module.price);
            //Wait Remove modele
        }
    }
    

    Shop shop;
    public void ReFreshShop()
    {
        shop.Refresh();
    }
    ModuleGeneration moduleGeneration;
}