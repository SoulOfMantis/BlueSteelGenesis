using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;
    public CharacterInfoTooltip characterTooltip;
    public ModuleInfoTooltip moduleTooltip;
    public CharacterTooltipTrigger currentCharacterTrigger;
    public ModuleTooltipTrigger currentModuleTrigger;
    public static readonly float HidingTimeInSeconds = .4f;
    public static readonly float ShowingTimeInSeconds = .2f;
    public static bool IsCharTooltipActive => instance.characterTooltip.enabled;
    public static bool IsModuleTooltipActive => instance.moduleTooltip.enabled;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        instance = this;
    }

    public static void Load(Character c)
    {
        instance.characterTooltip.updateInfo(c);
    }
    public static void Load(GameModule g)
    {
        instance.moduleTooltip.updateInfo(g);
    }

    public static void Show(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.characterTooltip:
                instance.characterTooltip.gameObject.SetActive(true);
                 break;
            case TooltipType.moduleTooltip:
                instance.moduleTooltip.gameObject.SetActive(true);
                Delay(TooltipType.characterTooltip);
                break;
        }
    }
    public static void Show(TooltipType type, CharacterTooltipTrigger trigger)
    {
        instance.currentCharacterTrigger = trigger;
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
            case TooltipType.characterTooltip:
                if (instance.currentCharacterTrigger)
                    instance.currentCharacterTrigger.StopCoroutine("HidingTooltip");
                break;
            case TooltipType.moduleTooltip:
                Delay(TooltipType.characterTooltip);
                if (instance.currentModuleTrigger)
                    instance.currentModuleTrigger.StopCoroutine("HidingTooltip");
                break;
        }
    }
    public static void ResumeHiding(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.characterTooltip:
                if (instance.currentCharacterTrigger)
                {
                    instance.currentCharacterTrigger.StopCoroutine("HidingTooltip");
                    instance.currentCharacterTrigger.StartCoroutine("HidingTooltip");
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
            case TooltipType.characterTooltip:
                instance.characterTooltip.gameObject.SetActive(false);               
                break;
            case TooltipType.moduleTooltip:
                instance.moduleTooltip.gameObject.SetActive(false);
                break;
        }

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public enum TooltipType
    { 
        characterTooltip, moduleTooltip
    }
}
