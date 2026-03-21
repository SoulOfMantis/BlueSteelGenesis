using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntityTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Entity entity = null;
    private Color baseEntityColor;
    IEnumerator ShowingTooltip()
    {
        if (TooltipSystem.IsEntityTooltipActive) yield return new WaitForSeconds(TooltipSystem.HidingTimeInSeconds);
        TooltipSystem.Load(entity);           
        yield return new WaitForSeconds(TooltipSystem.ShowingTimeInSeconds);
        TooltipSystem.Show(TooltipSystem.TooltipType.entityTooltip, this);
        if (entity != null) 
        { 
            entity.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            if (entity is Character c)
                Entity.tracker.HighlightCharacterInInitiative(c);
        }
    }
    IEnumerator HidingTooltip()
    {    
        yield return new WaitForSeconds(TooltipSystem.HidingTimeInSeconds);
        if (entity != null)  
        {
            entity.gameObject.GetComponent<SpriteRenderer>().color = baseEntityColor;
            if (entity is Character c)
                Entity.tracker.UnhighlightCharacterInInitiative(c);
        }
        TooltipSystem.Hide(TooltipSystem.TooltipType.entityTooltip);
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
        if (entity != null)  
            baseEntityColor = entity.gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
