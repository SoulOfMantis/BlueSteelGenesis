using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleTooltipTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Lock(TooltipSystem.TooltipType.moduleTooltip);
        TooltipSystem.Delay(TooltipSystem.TooltipType.moduleTooltip);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Unlock(TooltipSystem.TooltipType.moduleTooltip);
        TooltipSystem.ResumeHiding(TooltipSystem.TooltipType.moduleTooltip);
        TooltipSystem.ResumeHiding(TooltipSystem.TooltipType.entityTooltip);
    }
}
