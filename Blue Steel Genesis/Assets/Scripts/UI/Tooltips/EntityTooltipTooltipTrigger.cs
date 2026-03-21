using UnityEngine;
using UnityEngine.EventSystems;

public class EntityTooltipTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        TooltipSystem.Delay(TooltipSystem.TooltipType.entityTooltip);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Delay(TooltipSystem.TooltipType.entityTooltip);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.ResumeHiding(TooltipSystem.TooltipType.entityTooltip);
    }
}
