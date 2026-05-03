using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ModuleInfoTooltip : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Type;
    public TMP_Text Price;
    public TMP_Text Description;
    [SerializeField] Transform contentParent;
    private List<KeywordTooltip> entries = new();

    private RectTransform rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
    }
    public void updateInfo(GameModule g)
    {
        if (g == null) return;
        Name.text = g.Name;
        Description.text = g.Description();
        if (g is ActiveModule a) Type.text = $"Active: {a.energyCost} energy";
        else if (g is StatusModule) Type.text = "Status";
        else if (g is PassiveModule) Type.text = "Passive";

        if (ModuleGenerator.isBoss(g)) Price.text = "GoldenTicket";
        else Price.text = $"{g.price} gold";

            foreach (var entry in entries)
                Destroy(entry.gameObject);
        entries.Clear();
        foreach (var k in g.GetVisibleKeywords())
        {
            var go = Instantiate(TooltipSystem.KeywordTooltipPrefab, contentParent);
            var entry = go.GetComponent<KeywordTooltip>();
            entry.setup(k);
            entries.Add(entry);
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
