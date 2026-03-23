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
    public Button moduleButton;

    private List<ModuleEntry> entries = new List<ModuleEntry>();

    // Удалить или закоментировать при релизе, код для теста
    void Awake()
    {
        if (GameState.Run == null || GameState.Run.Expedition == null)
        {
            GameState.startGameRun(12345);
            GameState.Run.startExpedition(1);
        }
    }

    private void OnEnable()
    {
        ModuleManager.ModulesChanged += RefreshList;
        RefreshList();
        if (closeButton != null) closeButton.onClick.AddListener(CloseMenu);
        if (moduleButton != null) moduleButton.onClick.AddListener(GetNewModule);
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

    void GetNewModule()
    {
        var newModule = GameState.Run.Expedition.ModuleGen.GetNextCommonModule();
        ModuleManager.AddModule(newModule);
    }
}

