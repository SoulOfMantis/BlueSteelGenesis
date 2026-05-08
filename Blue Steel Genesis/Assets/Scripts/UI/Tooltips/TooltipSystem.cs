using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem instance;
    public EntityInfoTooltip entityTooltip;
    public ModuleInfoTooltip moduleTooltip;
    public EntityTooltipTrigger currentEntityTrigger;
    public static bool entityTooltipLocked = false;
    public ModuleTooltipTrigger currentModuleTrigger;
    public static bool moduleTooltipLocked = false;
    public static readonly float HidingTimeInSeconds = .4f;
    public static readonly float ShowingTimeInSeconds = .4f;
    public GameObject keywordTooltipPrefab;
    public static GameObject KeywordTooltipPrefab => instance.keywordTooltipPrefab;
    
    public static bool IsEntityTooltipActive()
    {
        if (instance == null) return false;
        if (instance.entityTooltip == null) return false;
        return instance.entityTooltip.enabled;
    }
    public static bool IsModuleTooltipActive()
    {
        if (instance == null) return false;
        if (instance.moduleTooltip == null) return false;
        return instance.moduleTooltip.enabled;
    }

    public void Awake()
    {
        instance = this;
        instance.entityTooltip.gameObject.SetActive(false);
        instance.moduleTooltip.gameObject.SetActive(false);
    }
    public static void Update(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.entityTooltip:
                Load(instance.currentEntityTrigger.entity);
                break;
            case TooltipType.moduleTooltip:
                Load(instance.currentModuleTrigger.module);
                break;
        }
    }
    public static bool IsCurrent(Entity e)
    {
        if (instance == null) return false;
        if (!IsEntityTooltipActive()) return false;
        if (instance.currentEntityTrigger == null) return false;
        return instance.currentEntityTrigger.entity == e;
    }
    public static bool IsCurrent(GameModule m)
    {
        if (instance == null) return false;
        if (!IsModuleTooltipActive()) return false;
        if (instance.currentModuleTrigger == null) return false;
        return instance.currentModuleTrigger.module == m;
    }

    public static void Load(Entity e)
    {
        instance.entityTooltip.updateInfo(e);
    }
    public static void Load(GameModule m)
    {
        instance.moduleTooltip.updateInfo(m);
    }
    public static void Show(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.entityTooltip:
                instance.entityTooltip.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(instance.entityTooltip.gameObject);
                break;
            case TooltipType.moduleTooltip:
                instance.moduleTooltip.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(instance.moduleTooltip.gameObject);
                break;
        }
    }
    public static void Show(TooltipType type, EntityTooltipTrigger trigger)
    {
        instance.currentEntityTrigger = trigger;
        instance.currentEntityTrigger.entity.changeColor(Color.yellow);
        Show(type);
    }
    public static void Show(TooltipType type, ModuleTooltipTrigger trigger)
    {
        instance.currentModuleTrigger = trigger;
        Show(type);
    }
  

    public static void Hide(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.entityTooltip:
                if (EventSystem.current.currentSelectedGameObject == instance.entityTooltip)
                    EventSystem.current.SetSelectedGameObject(null);
                break;
            case TooltipType.moduleTooltip:
                if (EventSystem.current.currentSelectedGameObject == instance.moduleTooltip)
                    EventSystem.current.SetSelectedGameObject(null);
                break;
        }

    }
    public enum TooltipType
    { 
        entityTooltip, moduleTooltip
    }
}
