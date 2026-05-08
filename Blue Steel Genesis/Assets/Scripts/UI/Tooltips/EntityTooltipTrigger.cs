using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntityTooltipTrigger : MonoBehaviour, IPointerClickHandler
{
    public Entity entity = null;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            TooltipSystem.Load(entity);
            TooltipSystem.Show(TooltipSystem.TooltipType.entityTooltip, this);
        }
    }
}
