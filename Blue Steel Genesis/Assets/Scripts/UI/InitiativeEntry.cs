using TMPro;
using UnityEngine;

public class InitiativeEntry : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text initiative;
    [SerializeField] EntityTooltipTrigger trigger;

    public void Setup(Character c)
    {
        nameText.text = c.Name;
        initiative.text = $"{c.Initiative}";
        trigger.entity = c;
    }
}
