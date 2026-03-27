using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntityInfoTooltip : MonoBehaviour
{
    [SerializeField] TMP_Text header;
    [SerializeField] TMP_Text description;
    [SerializeField] Slider energySlider;
    [SerializeField] TMP_Text energyDisplay;
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text healthDisplay;
    [SerializeField] Slider shieldSlider;
    [SerializeField] TMP_Text shieldDisplay;
    [SerializeField] List<ModuleTooltipTrigger> module_icons;
    [SerializeField] List<ModuleTooltipTrigger> status_icons;

    private RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
    }
    public void updateInfo(Entity entity)
    {
        if (entity == null) return;
        header.text = entity.Name;
        description.text = entity.Description;
        healthDisplay.text = $"{entity.currentHealth}/{entity.maxHealth}";
        healthSlider.maxValue = entity.maxHealth;
        healthSlider.value = entity.currentHealth;
        module_icons.ForEach(m => m.gameObject.SetActive(false));
        status_icons.ForEach(s => s.gameObject.SetActive(false));
        shieldSlider.gameObject.SetActive(false);
        energySlider.gameObject.SetActive(false);
        if (entity is Character c)
        {
            energySlider.gameObject.SetActive(true);
            if (c.currentShield != 0)
            {
                shieldSlider.gameObject.SetActive(true);
                shieldDisplay.text = $"{c.currentShield}";
                shieldSlider.value = c.currentShield;
            }
            energyDisplay.text = $"{c.currentEnergy}/{c.maxEnergy}";
            energySlider.maxValue = c.maxEnergy;
            energySlider.value = c.currentEnergy;
            for (int i = 0; i < c.Modules.Count; i++)
            {
                module_icons[i].gameObject.SetActive(true);
                module_icons[i].updateModuleTrigger(c.Modules[i]);
            }
            for (int i = 0; i < c.Statuses.Count; i++)
            {
                Debug.Log($"Updating status trigger {i}");
                status_icons[i].gameObject.SetActive(true);
                status_icons[i].updateModuleTrigger(c.Statuses[i]);
            }
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
