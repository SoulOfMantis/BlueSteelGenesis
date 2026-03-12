using UnityEngine;
using System.Collections.Generic;


public class testShopManager : MonoBehaviour
{
    [SerializeField] GameObject sellMenu;
    [SerializeField] List<ModuleTooltipTrigger> playerModuleIcons;
    [SerializeField] List<ModuleTooltipTrigger> shopModuleIcons;
    private void Start()
    {
        RerollShop();
    }
    void updatePlayerModuleIcons()
    {
        for (int i = 0; i < playerModuleIcons.Count; i++)
        {
            playerModuleIcons[i].gameObject.SetActive((i + 1 < GameState.Run.Expedition.Player.modules.Count) && GameState.Run.Expedition.Player.modules[i + 1] != null);
            if (playerModuleIcons[i].gameObject.activeSelf) playerModuleIcons[i].updateModuleTrigger(GameState.Run.Expedition.Player.modules[i + 1]);
        }
    }
    void updateShopModuleIcons()
    {
        for (int i = 0; i < shopModuleIcons.Count; i++)
        {
            shopModuleIcons[i].gameObject.SetActive(i < GameState.Run.Expedition.Shop.OnSale.Count);
            if (shopModuleIcons[i].gameObject.activeSelf) shopModuleIcons[i].updateModuleTrigger(GameState.Run.Expedition.Shop.OnSale[i]);
        }
    }
    void UpdateShop()
    {
        updatePlayerModuleIcons();
        updateShopModuleIcons();
        //TODO: update icons and disable unused objects
    }
    public void ExitToMap() => UnityEngine.SceneManagement.SceneManager.LoadScene("ExpeditionMapTest_usingGameState");
    public void RerollShop()
    {
        GameState.Run.Expedition.Shop.Reroll();
        UpdateShop();
    }    
    public void BuyModuleNumber(int index)
    {
        if (index < 0 || index >= GameState.Run.Expedition.Shop.OnSale.Count) return;
        GameState.Run.Expedition.Shop.Buy(GameState.Run.Expedition.Shop.OnSale[index]);
        UpdateShop();
    }
    public void SellModuleNumber(int index)
    {
        if (index <= 0 || index >= GameState.Run.Expedition.Player.modules.Count) return;
        GameState.Run.Expedition.Shop.Sell(GameState.Run.Expedition.Player.modules[index]);
        UpdateShop();
    }
    public void ToggleSellMenu() => sellMenu.SetActive(!sellMenu.activeSelf);
}
