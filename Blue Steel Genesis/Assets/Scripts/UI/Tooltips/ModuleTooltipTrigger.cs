using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameModule module;

    IEnumerator ShowingTooltip()
    {
        TooltipSystem.Load(module);           
        yield return new WaitForSeconds(2f);
        TooltipSystem.Show(TooltipSystem.TooltipType.moduleTooltip);
    }
    IEnumerator HidingTooltip()
    {
        yield return new WaitForSeconds(.5f);
        TooltipSystem.Hide(TooltipSystem.TooltipType.moduleTooltip);
    }
    public void OnMouseEnter()
    {
        StopCoroutine("HidingTooltip");
        StartCoroutine("ShowingTooltip");
    }

    public void OnMouseExit()
    {
        StopCoroutine("ShowingTooltip");
        StartCoroutine("HidingTooltip");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopCoroutine("HidingTooltip");
        StartCoroutine("ShowingTooltip");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine("ShowingTooltip");
        StartCoroutine("HidingTooltip");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
