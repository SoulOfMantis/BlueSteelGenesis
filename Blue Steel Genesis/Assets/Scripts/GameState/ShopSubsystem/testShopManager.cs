using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class testShopManager : MonoBehaviour
{
    [SerializeField] List<ModuleTooltipTrigger> shopModuleIcons;
    [SerializeField] List<TMP_Text> shopModulePrices;
    [SerializeField] TMP_Text rerollPrice;
    [SerializeField] RerollOptions ShopMode;
    [SerializeField] TryButtonSFX buy_button_sfx;
    [SerializeField] TryButtonSFX reroll_button_sfx;

    private void Start()
    {
        UpdateShop();
    }
    void updateShopModuleIcons()
    {
        for (int i = 0; i < shopModuleIcons.Count; i++)
        {
            shopModuleIcons[i].gameObject.SetActive(i < GameState.Run.Expedition.Shop.OnSale.Count && GameState.Run.Expedition.Shop.OnSale[i] != null);
            if (shopModuleIcons[i].gameObject.activeSelf)
            {
                shopModuleIcons[i].updateModuleTrigger(GameState.Run.Expedition.Shop.OnSale[i]);
                switch(ShopMode)
                {
                    case RerollOptions.Common:
                        shopModulePrices[i].text = $"Buy for {GameState.Run.Expedition.Shop.OnSale[i].price}";
                        break;
                    case RerollOptions.Boss:
                        shopModulePrices[i].text = "Buy for a golden ticket";
                        break;
                }
            }
        }
    }
    void UpdateShop()
    {
        updateShopModuleIcons();
        rerollPrice.text = $"Price: {GameState.Run.Expedition.Shop.RerollCost}";
    }
    public void ExitToMap() => GameState.Run.Expedition.exitNode();
    public void RerollShop()
    {
        if (GameState.Run.Expedition.Shop.Reroll(ShopMode))
            reroll_button_sfx.playSuccess();
        else
            reroll_button_sfx.playFailure();
        UpdateShop();
    }    
    public void BuyModuleNumber(int index)
    {
        if (index < 0 || index >= GameState.Run.Expedition.Shop.OnSale.Count) return;
        if (ModuleManager.BuyModule(GameState.Run.Expedition.Shop.OnSale[index]))
            buy_button_sfx.playSuccess();
        else
            buy_button_sfx.playFailure();
        UpdateShop();
    }
}
