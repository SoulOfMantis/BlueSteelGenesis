using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;
    public CharacterInfoTooltip tooltip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        current = this;
    }

    public static void Load(Character c)
    {
        current.tooltip.updateText(c);
    }
    public static void Show()
    {
        current.tooltip.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
