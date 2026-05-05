using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModuleManagementUI : MonoBehaviour
{
    [SerializeField] GameObject moduleEntryPrefab;
    [SerializeField] Transform contentParent;
    [SerializeField] TMP_Text moneyDisplay;
    [SerializeField] TMP_Text materialsDisplay;
    [SerializeField] TMP_Text ticketsDisplay;
    [SerializeField] bool InShop = false;
    private List<ModuleEntry> entries = new List<ModuleEntry>();
    public bool UpgradeMode = false;

    private void OnEnable()
    {
        ModuleManager.ModulesChanged += RefreshList;
        ModuleManager.MoneyChanged += RefreshMoney;
        ModuleManager.MaterialsChanged += RefreshMaterials;
        ModuleManager.TicketsChanged += RefreshTickets;
        RefreshList();
        RefreshMoney();
        RefreshMaterials();
        RefreshTickets();
    }
    void RefreshMoney() => moneyDisplay.text = $"Money: {GameState.Run.Expedition.Player.money}";
    void RefreshMaterials() => materialsDisplay.text = $"Spare parts: {GameState.Run.Expedition.Player.materials}";
    void RefreshTickets() => ticketsDisplay.text = $"Golden Tickets: {GameState.Run.Expedition.Player.GoldenTickets}";
    void RefreshList()
    {
        foreach (var entry in entries)
            if (entry != null) Destroy(entry.gameObject);
        entries.Clear();

        for (int i = 0; i <= ModuleManager.MaxEditableModuleIndex; i++)
        {
            var go = Instantiate(moduleEntryPrefab, contentParent);
            var entry = go.GetComponent<ModuleEntry>();
            entry.Setup(ModuleManager.Modules[i], (uint)i, this, InShop, UpgradeMode);
            entries.Add(entry);
        }
    }

    public void MoveModuleUp(uint idx) => ModuleManager.MoveModuleUp(idx);
    public void MoveModuleDown(uint idx) => ModuleManager.MoveModuleDown(idx);
    public void RemoveModule(uint idx) => ModuleManager.RemoveModule(idx);
    public void SellModule(uint idx) => ModuleManager.SellModule(idx);
    public void UpgradeModule(uint idx) => ModuleManager.UpgradeModule(idx);
}

