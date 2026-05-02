using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class testShopManager : MonoBehaviour
{
    [SerializeField] List<ModuleTooltipTrigger> shopModuleIcons;
    [SerializeField] TMP_Text rerollPrice;
    [SerializeField] RerollOptions ShopMode;
    private void Start()
    {
        RerollShop();
    }
    void updateShopModuleIcons()
    {
        for (int i = 0; i < shopModuleIcons.Count; i++)
        {
            shopModuleIcons[i].gameObject.SetActive(i < GameState.Run.Expedition.Shop.OnSale.Count && GameState.Run.Expedition.Shop.OnSale[i] != null);
            if (shopModuleIcons[i].gameObject.activeSelf) shopModuleIcons[i].updateModuleTrigger(GameState.Run.Expedition.Shop.OnSale[i]);
        }
    }
    void UpdateShop()
    {
        updateShopModuleIcons();
    }
    public void ExitToMap() => GameState.Run.Expedition.exitNode();
    public void RerollShop()
    {
        GameState.Run.Expedition.Shop.Reroll(ShopMode);
        rerollPrice.text = $"Price: {GameState.Run.Expedition.Shop.RerollCost}";
        UpdateShop();
    }    
    public void BuyModuleNumber(int index)
    {
        if (index < 0 || index >= GameState.Run.Expedition.Shop.OnSale.Count) return;
        GameState.Run.Expedition.Shop.Buy(GameState.Run.Expedition.Shop.OnSale[index]);
        UpdateShop();
    }
}
