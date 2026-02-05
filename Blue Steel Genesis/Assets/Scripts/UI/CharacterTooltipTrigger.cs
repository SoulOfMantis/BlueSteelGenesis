using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterTooltipTrigger : MonoBehaviour
{
    public Character character;
    public void OnMouseEnter()
    {
        StartCoroutine("ShowingTooltip");
    }

    IEnumerator ShowingTooltip()
    {
        yield return new WaitForSeconds(2.5f);
        TooltipSystem.Show(character);
    }
    public void OnMouseExit()
    {
        StopCoroutine("ShowingTooltip");
        TooltipSystem.Hide();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
