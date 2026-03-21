using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleTooltipTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Delay(TooltipSystem.TooltipType.moduleTooltip);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.ResumeHiding(TooltipSystem.TooltipType.moduleTooltip);
    }
    private void OnMouseEnter()
    {
        TooltipSystem.Delay(TooltipSystem.TooltipType.moduleTooltip);
    }
    private void OnMouseExit()
    {
        TooltipSystem.ResumeHiding(TooltipSystem.TooltipType.moduleTooltip);
    }
}
