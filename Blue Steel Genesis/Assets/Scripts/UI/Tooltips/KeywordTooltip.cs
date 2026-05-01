using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeywordTooltip : MonoBehaviour
{
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Description;
    [SerializeField] ModuleTooltipTrigger tooltipTrigger;
    VisibleKeyword keyword;

    public void setup(VisibleKeyword k) 
    {
        if (k == null) return;
        keyword = k;
        Name.text = k.Name;
        Description.text = k.Description;
        if (k is TargetedStatusKeyword t) tooltipTrigger.updateModuleTrigger(t.Status);
        else tooltipTrigger.gameObject.SetActive(false);
    }
}
