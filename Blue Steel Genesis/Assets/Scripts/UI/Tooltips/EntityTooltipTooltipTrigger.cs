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
        TooltipSystem.Lock(TooltipSystem.TooltipType.entityTooltip);
        TooltipSystem.Delay(TooltipSystem.TooltipType.entityTooltip);
    }    
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Unlock(TooltipSystem.TooltipType.entityTooltip);
        TooltipSystem.ResumeHiding(TooltipSystem.TooltipType.entityTooltip);
    }
}
