using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;
    public CharacterInfoTooltip characterTooltip;
    public ModuleInfoTooltip moduleTooltip;
    public CharacterTooltipTrigger current;

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
                {
                    instance.characterTooltip.gameObject.SetActive(true);
                    break;
                }
            case TooltipType.moduleTooltip:
                {
                    instance.moduleTooltip.gameObject.SetActive(true);
                    break;
                }
        }
    }
    public static void Show(TooltipType type, CharacterTooltipTrigger trigger)
    {
        instance.current = trigger;
        Show(type);
    }
    public static void Delay()
    {
        instance.current.StopCoroutine("HidingTooltip");
    }
    public static void ResumeHiding()
    {
        instance.current.StopCoroutine("HidingTooltip");
        instance.current.StartCoroutine("HidingTooltip");
    }

    public static void Hide(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.characterTooltip:
                {
                    instance.characterTooltip.gameObject.SetActive(false);
                    break;
                }
            case TooltipType.moduleTooltip:
                {
                    instance.moduleTooltip.gameObject.SetActive(false);
                    break;
                }
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
