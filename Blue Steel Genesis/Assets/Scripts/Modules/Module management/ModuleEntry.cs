using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class ModuleEntry : MonoBehaviour
{
    public TMP_Text nameText;
    public Button upButton;
    public Button downButton;

    private int currentIdx;
    private ModuleManagemntUI manager;

    public void Setup(GameModule module, int idx, ModuleManagemntUI mgr)
    {
        currentIdx = idx;
        manager = mgr;

        nameText.text = module.Name;
        
        upButton.onClick.RemoveAllListeners();
        downButton.onClick.RemoveAllListeners();

        upButton.onClick.AddListener(() => manager.MoveModuleUp(currentIdx));
        downButton.onClick.AddListener(() => manager.MoveModuleDown(currentIdx));
        upButton.interactable = idx > 0;
        downButton.interactable = idx < ModuleManager.Modules.Count - 1;
    }
}
