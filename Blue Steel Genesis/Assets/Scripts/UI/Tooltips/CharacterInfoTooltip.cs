using UnityEngine;
using TMPro;
using UnityEditor;

public class CharacterInfoTooltip : MonoBehaviour
{
    public TMP_Text header;
    public TMP_Text description;
    public TMP_Text health;
    public TMP_Text shield;
    public TMP_Text energy;

    private RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
    }
    public void updateText(Character c)
    {
        if (c != null)
        {
            header.text = c.Name;
            description.text = c.Description;       
            shield.text = $"{c.currentShield}"; 
            health.text = $"{c.currentHealth}/{c.maxHealth}";
            energy.text = $"{c.currentEnergy}/{c.maxEnergy}";
        }
    }

    void OnEnable()
    {
        Vector2 position = Input.mousePosition;
        transform.position = position;
        if (transform.position.x >= Screen.width / 2) //x в правой половине экрана
            SetPivotRight();
        else SetPivotLeft();
        if (transform.position.y >= Screen.height / 2) //y в верхней половине экрана
            SetPivotUp();
        else SetPivotDown();
    }
    void SetPivotLeft()
    {
        rectTransform.pivot = new(0, rectTransform.pivot.y);
    }
    void SetPivotRight()
    {
        rectTransform.pivot = new(1, rectTransform.pivot.y);
    }
    void SetPivotUp()
    {
        rectTransform.pivot = new(rectTransform.pivot.x, 1);
    }
    void SetPivotDown()
    {
        rectTransform.pivot = new(rectTransform.pivot.x, 0);
    }

}
