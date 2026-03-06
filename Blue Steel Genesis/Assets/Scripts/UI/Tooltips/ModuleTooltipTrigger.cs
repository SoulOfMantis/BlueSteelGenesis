using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ModuleTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameModule module;
    public Image icon;
    IEnumerator ShowingTooltip()
    {
        if (TooltipSystem.IsModuleTooltipActive) yield return new WaitForSeconds(0.5f);
        TooltipSystem.Load(module);           
        yield return new WaitForSeconds(0.5f);
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
    public void updateModuleTrigger(GameModule module)
    {
        this.module = module;
        updateIcon();
    }
    public void updateIcon() =>  icon.overrideSprite = Resources.Load<Sprite>($"ModuleIcons/{module.Icon_name}");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
