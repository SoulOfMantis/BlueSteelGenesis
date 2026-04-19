using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;
    public EntityInfoTooltip entityTooltip;
    public ModuleInfoTooltip moduleTooltip;
    EntityTooltipTrigger currentEntityTrigger;
    public static bool entityTooltipLocked = false;
    ModuleTooltipTrigger currentModuleTrigger;
    public static bool moduleTooltipLocked = false;
    public static readonly float HidingTimeInSeconds = .4f;
    public static readonly float ShowingTimeInSeconds = .4f;
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
    public static void Lock(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.entityTooltip:
                entityTooltipLocked = true;
                break;
            case TooltipType.moduleTooltip:
                moduleTooltipLocked = true;
                break;
        }
    }
    public static void Unlock(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.entityTooltip:
                entityTooltipLocked = false;
                break;
            case TooltipType.moduleTooltip:
                moduleTooltipLocked = false;
                break;
        }
    }

    public static void Show(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.entityTooltip:
                instance.entityTooltip.gameObject.SetActive(true);
                 break;
            case TooltipType.moduleTooltip:
                instance.moduleTooltip.gameObject.SetActive(true);
                break;
        }
    }
    public static void Show(TooltipType type, EntityTooltipTrigger trigger)
    {
        instance.currentEntityTrigger = trigger;
        Show(type);
    }
    public static void Show(TooltipType type, ModuleTooltipTrigger trigger)
    {
        instance.currentModuleTrigger = trigger;
        Show(type);
    }
    public static void Delay(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.entityTooltip:
                if (instance.currentEntityTrigger)
                    instance.currentEntityTrigger.StopCoroutine("HidingTooltip");
                break;
            case TooltipType.moduleTooltip:
                Delay(TooltipType.entityTooltip);
                if (instance.currentModuleTrigger)
                    instance.currentModuleTrigger.StopCoroutine("HidingTooltip");
                break;
        }
    }
    public static void ResumeHiding(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.entityTooltip:
                if (instance.currentEntityTrigger)
                {
                    instance.currentEntityTrigger.StopCoroutine("HidingTooltip");
                    instance.currentEntityTrigger.StartCoroutine("HidingTooltip");
                }                
                break;
            case TooltipType.moduleTooltip:
                if (instance.currentModuleTrigger)
                {   
                    instance.currentModuleTrigger.StopCoroutine("HidingTooltip");
                    instance.currentModuleTrigger.StartCoroutine("HidingTooltip");
                }     
                break;
        }
    }

    public static void Hide(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.entityTooltip:
                instance.entityTooltip.gameObject.SetActive(false);               
                break;
            case TooltipType.moduleTooltip:
                instance.moduleTooltip.gameObject.SetActive(false);
                Unlock(TooltipType.entityTooltip);
                break;
        }

    }
    public enum TooltipType
    { 
        entityTooltip, moduleTooltip
    }
}
