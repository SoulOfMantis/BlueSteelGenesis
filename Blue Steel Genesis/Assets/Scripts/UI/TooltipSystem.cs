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

    public static void Show(Character c)
    {
        current.tooltip.gameObject.SetActive(true);
        current.tooltip.updateText(c);
    }
    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
    public static void Toggle()
    {
        var g = current.tooltip.gameObject;
        g.SetActive(!g.activeSelf);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
