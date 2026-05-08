using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleTooltipTooltipTrigger : MonoBehaviour, IDeselectHandler
{
    public EntityTooltipTooltipTrigger tooltip;
    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        gameObject.SetActive(false);
        if (tooltip != null)
        {
            tooltip.Deselect();
        }
    }
}
