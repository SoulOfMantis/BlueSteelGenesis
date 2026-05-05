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

    public void Setup(GameModule module, uint idx, ModuleManagementUI mgr, bool InShop = false, bool upgradeMode = false)
    {
        currentIdx = idx;
        manager = mgr;

        nameText.text = upgradeMode ? $"{module.Name} [Lv.{module.upgradeLevel}/{module.maxUpgradeLevel}]" : module.Name;
        
        upButton.onClick.RemoveAllListeners();
        downButton.onClick.RemoveAllListeners();

        if (!upgradeMode)
        {
            upButton.onClick.AddListener(() => manager.MoveModuleUp(currentIdx));
            downButton.onClick.AddListener(() => manager.MoveModuleDown(currentIdx));

            upButton.gameObject.SetActive(idx <= ModuleManager.MaxEditableModuleIndex && idx > ModuleManager.MinEditableModuleIndex);
            downButton.gameObject.SetActive(idx < ModuleManager.MaxEditableModuleIndex && idx >= ModuleManager.MinEditableModuleIndex);
        }
        else
        {
            upButton.gameObject.SetActive(false); 
            downButton.gameObject.SetActive(false);
        }

        if (!upgradeMode)
        {
            if (InShop && !ModuleGenerator.isBoss(module))
            {
                removeButton.image.color = Color.gold;
                var tmp = removeButton.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
                tmp.text = $"Sell for {module.price / 2}";
                removeButton.onClick.AddListener(() => manager.SellModule(currentIdx));
            }
            else
            {
                var tmp = removeButton.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
                tmp.text = "Remove";
                removeButton.onClick.AddListener(() => manager.RemoveModule(currentIdx));
            }

            removeButton.gameObject.SetActive(
                idx <= ModuleManager.MaxEditableModuleIndex && idx >= ModuleManager.MinEditableModuleIndex);
        }
        else
        {
            bool canUpgrade = module.CanUpgrade;
            uint cost = canUpgrade ? module.GetUpgradeCost() : 0;
            bool hasMaterials = GameState.Run.Expedition.Player.HasEnoughMaterials(cost);

            removeButton.gameObject.SetActive(true);
            var tmp = removeButton.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
            tmp.text = canUpgrade ? $"Upgrade ({cost} mats)" : "Max Lvl";

            removeButton.interactable = canUpgrade && hasMaterials;
            removeButton.onClick.AddListener(() => manager.UpgradeModule(currentIdx));
        }

        trigger.updateModuleTrigger(module);
    }
}

