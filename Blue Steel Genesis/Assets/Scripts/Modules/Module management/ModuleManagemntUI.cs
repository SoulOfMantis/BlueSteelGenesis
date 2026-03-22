using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


class ModuleManagemntUI : MonoBehaviour
{
    public GameObject moduleEntryPrefab;
    public Transform contentParent;
    public Button closeButton;

    private List<ModuleEntry> entries = new List<ModuleEntry>();

    private void OnEnable()
    {
        ModuleManager.ModulesChanged += RefreshList;
    }

    void RefreshList()
    {
        foreach (var entry in entries)
            Destroy(entry.gameObject);
        entries.Clear();

        var modules = ModuleManager.Modules;
        for (int i = 0; i < modules.Count; i++)
        {
            var go = Instantiate(moduleEntryPrefab, contentParent);
            var entry = go.GetComponent<ModuleEntry>();
            entry.Setup(modules[i], i, this);
            entries.Add(entry);
        }
    }

    public void MoveModuleUp(int idx) => ModuleManager.MoveModuleUp(idx);
    public void MoveModuleDown(int idx) => ModuleManager.MoveModuleDown(idx);

    void CloseMenu()
    {
        return;
    }
}

