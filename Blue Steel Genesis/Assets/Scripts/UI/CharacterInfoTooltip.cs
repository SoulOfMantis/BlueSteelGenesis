using UnityEngine;
using TMPro;

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
        header.text = c.Name;
        description.text = c.Description;
        shield.text = $"{c.currentShield}";
        health.text = $"{c.currentHealth}/{c.maxHealth}";
        energy.text = $"{c.currentEnergy}/{c.maxEnergy}";
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 position = Input.mousePosition;
        var pivX = position.x / Screen.width;
        var pivY = position.y / Screen.height;
        rectTransform.pivot = new(pivX, pivY);
        transform.position = position;
    }
}
