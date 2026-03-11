using UnityEngine;
using System.Collections.Generic;


public class testShopManager : MonoBehaviour
{
    [SerializeField] GameObject sellMenu;
    [SerializeField] List<GameObject> playerModuleIcons;
    [SerializeField] List<GameObject> shopModuleIcons;
    private void Awake()
    {
        RefreshShop();
    }
    void updatePlayerModuleIcons()
    {
        for (int i = 0; i < playerModuleIcons.Count; i++)
        {
            playerModuleIcons[i].SetActive((i+1 < GameState.Run.Expedition.Player.modules.Count) && GameState.Run.Expedition.Player.modules[i+1] != null);
            //TODO: update icons!
        }
    }
    void updateShopModuleIcons()
    {
        for (int i = 0; i < shopModuleIcons.Count; i++)
        {
            shopModuleIcons[i].SetActive(i < GameState.Run.Expedition.Shop.OnSale.Count);
            //TODO: update icons!
        }
    }
    void UpdateShop()
    {
        updatePlayerModuleIcons();
        updateShopModuleIcons();
        //TODO: update icons and disable unused objects
    }
    public void ExitToMap() => UnityEngine.SceneManagement.SceneManager.LoadScene("ExpeditionMapTest_usingGameState");
    public void RefreshShop()
    {
        GameState.Run.Expedition.Shop.Refresh();
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
