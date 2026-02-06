using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public CharacterInfoTooltip characterTooltip;
    public ModuleInfoTooltip moduleTooltip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        current = this;
    }

    public static void Load(Character c)
    {
        current.characterTooltip.updateText(c);
    }
    public static void Load(GameModule g)
    {
        current.moduleTooltip.updateInfo(g);
    }

    public static void Show(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.characterTooltip:
                {
                    current.characterTooltip.gameObject.SetActive(true);
                    break;
                }
            case TooltipType.moduleTooltip:
                {
                    current.moduleTooltip.gameObject.SetActive(true);
                    break;
                }
        }
    }
    public static void Hide(TooltipType type)
    {
        switch (type)
        {
            case TooltipType.characterTooltip:
                {
                    current.characterTooltip.gameObject.SetActive(false);
                    break;
                }
            case TooltipType.moduleTooltip:
                {
                    current.moduleTooltip.gameObject.SetActive(false);
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
