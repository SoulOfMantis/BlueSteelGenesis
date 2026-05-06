using TMPro;
using UnityEngine;
using UnityEngine.UI;

class ModuleEntry : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] Button upButton;
    [SerializeField] Button downButton;
    [SerializeField] Button actionButton;
    [SerializeField] ModuleTooltipTrigger trigger;

    private uint currentIdx;
    private ModuleManagementUI manager;

    public void Setup(GameModule module, uint idx, ModuleManagementUI mgr, ModuleManager.InventoryOptions mode = ModuleManager.InventoryOptions.Remove)
    {
        currentIdx = idx;
        manager = mgr;
        upButton.onClick.RemoveAllListeners();
        downButton.onClick.RemoveAllListeners();
        switch (mode)
        {
            case ModuleManager.InventoryOptions.Remove:
                SetupRemove(module);
                break;
            case ModuleManager.InventoryOptions.Sell:
                SetupSell(module);
                break;
        }
        actionButton.gameObject.SetActive(idx <= ModuleManager.MaxEditableModuleIndex && idx >= ModuleManager.MinEditableModuleIndex);
        trigger.updateModuleTrigger(module);
    }

    void SetupRemove(GameModule module)
        {
            nameText.text = module.Name;
            upButton.onClick.AddListener(() => manager.MoveModuleUp(currentIdx));
            downButton.onClick.AddListener(() => manager.MoveModuleDown(currentIdx));
            upButton.gameObject.SetActive(currentIdx <= ModuleManager.MaxEditableModuleIndex && currentIdx > ModuleManager.MinEditableModuleIndex);
            downButton.gameObject.SetActive(currentIdx < ModuleManager.MaxEditableModuleIndex && currentIdx >= ModuleManager.MinEditableModuleIndex);
            
            var tmp = actionButton.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
            tmp.text = "Remove";
            actionButton.onClick.AddListener(() => manager.RemoveModule(currentIdx));
        }
        void SetupSell(GameModule module)        
        {
            SetupRemove(module);
            if (!ModuleGenerator.isBoss(module))
            {
                actionButton.image.color = Color.gold;
                var tmp = actionButton.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
                tmp.text = $"Sell for {module.price / 2}";
                actionButton.onClick.AddListener(() => manager.SellModule(currentIdx));
            }
        }
}
