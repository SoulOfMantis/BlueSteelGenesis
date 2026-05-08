using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityTooltipTooltipTrigger : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Entity current;
    public List<ModuleTooltipTrigger> myTriggers;
    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        current.changeColor(Color.yellow);
    }
    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        if (!myTriggers.Contains(TooltipSystem.instance.currentModuleTrigger)) Deselect();
        else TooltipSystem.instance.moduleTooltip.trigger.tooltip = this;
    }
    public void Deselect()
    {
        current.unchangeColor();
        gameObject.SetActive(false);
    }
}
