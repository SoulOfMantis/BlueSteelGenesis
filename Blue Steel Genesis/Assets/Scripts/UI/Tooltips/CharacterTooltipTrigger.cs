using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Character character = null;
    private Color baseCharacterColor;
    IEnumerator ShowingTooltip()
    {
        TooltipSystem.Load(character);           
        yield return new WaitForSeconds(2f);
        TooltipSystem.Show(TooltipSystem.TooltipType.characterTooltip, this);
        if (character != null)  
        { 
            character.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            Character.tracker.HighlightCharacterInInitiative(character);
        }
    }
    IEnumerator HidingTooltip()
    {    
        yield return new WaitForSeconds(.5f);
        if (character != null)  
        {
            character.gameObject.GetComponent<SpriteRenderer>().color = baseCharacterColor;
            Character.tracker.UnhighlightCharacterInInitiative(character);
        }        
        TooltipSystem.Hide(TooltipSystem.TooltipType.characterTooltip);
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
        if (character != null)  
            baseCharacterColor = character.gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
