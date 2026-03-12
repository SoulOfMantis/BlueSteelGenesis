using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterTooltipTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Delay();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.ResumeHiding();
    }
    private void OnMouseEnter()
    {
        TooltipSystem.Delay();
    }
    private void OnMouseExit()
    {
        TooltipSystem.ResumeHiding();
    }
}
