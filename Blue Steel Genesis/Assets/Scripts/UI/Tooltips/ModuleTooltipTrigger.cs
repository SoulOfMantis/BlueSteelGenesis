using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ModuleTooltipTrigger : MonoBehaviour, IPointerClickHandler
{
    public GameModule module;
    public Image icon;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            TooltipSystem.Load(module);
            TooltipSystem.Show(TooltipSystem.TooltipType.moduleTooltip, this);
        }
    }
    public void updateModuleTrigger(GameModule module)
    {
        this.module = module;
        updateIcon();
    }
    public void updateIcon()
    {
        if (module == null) return;
        icon.overrideSprite = Resources.Load<Sprite>($"ModuleIcons/{module.Icon_name}");
    }
    void Awake()
    {
        if (icon == null) icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
