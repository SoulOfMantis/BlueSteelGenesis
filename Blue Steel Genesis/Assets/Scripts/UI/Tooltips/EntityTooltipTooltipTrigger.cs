using UnityEngine;
using UnityEngine.EventSystems;

public class EntityTooltipTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Delay(TooltipSystem.TooltipType.entityTooltip);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.ResumeHiding(TooltipSystem.TooltipType.entityTooltip);
    }
    private void OnMouseEnter()
    {
        TooltipSystem.Delay(TooltipSystem.TooltipType.entityTooltip);
    }
    private void OnMouseExit()
    {
        TooltipSystem.ResumeHiding(TooltipSystem.TooltipType.entityTooltip);
    }
}
