using TMPro;
using UnityEngine;
using UnityEngine.UI;

class ModuleEntry : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] Button upButton;
    [SerializeField] Button downButton;
    [SerializeField] Button removeButton;
    [SerializeField] ModuleTooltipTrigger trigger;

    private uint currentIdx;
    private ModuleManagementUI manager;

    public void Setup(GameModule module, uint idx, ModuleManagementUI mgr, bool InShop = false)
    {
        currentIdx = idx;
        manager = mgr;

        nameText.text = module.Name;
        
        upButton.onClick.RemoveAllListeners();
        downButton.onClick.RemoveAllListeners();

        upButton.onClick.AddListener(() => manager.MoveModuleUp(currentIdx));
        downButton.onClick.AddListener(() => manager.MoveModuleDown(currentIdx));
        if (InShop && !ModuleGenerator.isBoss(module))
        {
            removeButton.image.color = Color.gold;
            (removeButton.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text).text = $"Sell for {module.price/2}";
            removeButton.onClick.AddListener(() => manager.SellModule(currentIdx));
        }
        else
            removeButton.onClick.AddListener(() => manager.RemoveModule(currentIdx));

        upButton.gameObject.SetActive(idx <= ModuleManager.MaxEditableModuleIndex && idx > ModuleManager.MinEditableModuleIndex);
        downButton.gameObject.SetActive(idx < ModuleManager.MaxEditableModuleIndex && idx >= ModuleManager.MinEditableModuleIndex);
        removeButton.gameObject.SetActive(idx <= ModuleManager.MaxEditableModuleIndex && idx >= ModuleManager.MinEditableModuleIndex);

        trigger.updateModuleTrigger(module);
    }
}
