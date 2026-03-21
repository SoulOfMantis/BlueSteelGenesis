using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterTooltipTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Delay(TooltipSystem.TooltipType.characterTooltip);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.ResumeHiding(TooltipSystem.TooltipType.characterTooltip);
    }
    private void OnMouseEnter()
    {
        TooltipSystem.Delay(TooltipSystem.TooltipType.characterTooltip);
    }
    private void OnMouseExit()
    {
        TooltipSystem.ResumeHiding(TooltipSystem.TooltipType.characterTooltip);
    }
}
