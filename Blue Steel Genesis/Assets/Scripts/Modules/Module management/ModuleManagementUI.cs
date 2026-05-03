using System.Collections.Generic;
using UnityEngine;

public class ModuleManagementUI : MonoBehaviour
{
    [SerializeField] GameObject moduleEntryPrefab;
    [SerializeField] Transform contentParent;
    [SerializeField] bool InShop = false;
    private List<ModuleEntry> entries = new List<ModuleEntry>();

    private void OnEnable()
    {
        ModuleManager.ModulesChanged += RefreshList;
        RefreshList();
    }
    void RefreshList()
    {
        foreach (var entry in entries)
            if (entry != null) Destroy(entry.gameObject);
        entries.Clear();

        for (int i = 0; i <= ModuleManager.MaxEditableModuleIndex; i++)
        {
            var go = Instantiate(moduleEntryPrefab, contentParent);
            var entry = go.GetComponent<ModuleEntry>();
            entry.Setup(ModuleManager.Modules[i], (uint)i, this, InShop);
            entries.Add(entry);
        }
    }

    public void MoveModuleUp(uint idx) => ModuleManager.MoveModuleUp(idx);
    public void MoveModuleDown(uint idx) => ModuleManager.MoveModuleDown(idx);
    public void RemoveModule(uint idx) => ModuleManager.RemoveModule(idx);
    public void SellModule(uint idx) => ModuleManager.SellModule(idx);
}

