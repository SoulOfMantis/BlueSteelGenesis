using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Character character;
    private Color baseCharacterColor;
    public void OnMouseEnter()
    {
        StartCoroutine("ShowingTooltip");
    }

    IEnumerator ShowingTooltip()
    {
        TooltipSystem.Load(character);
        yield return new WaitForSeconds(2f);
        TooltipSystem.Show();
        character.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        Character.tracker.HighlightCharacterInInitiative(character);
    }
    public void OnMouseExit()
    {
        StopCoroutine("ShowingTooltip");
        character.gameObject.GetComponent<SpriteRenderer>().color = baseCharacterColor;
        Character.tracker.UnhighlightCharacterInInitiative(character);
        TooltipSystem.Hide();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine("ShowingTooltip");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StopCoroutine("ShowingTooltip");
        character.gameObject.GetComponent<SpriteRenderer>().color = baseCharacterColor;
        Character.tracker.UnhighlightCharacterInInitiative(character);
        TooltipSystem.Hide();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseCharacterColor = character.gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
