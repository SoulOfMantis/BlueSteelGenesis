using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NodeButton : MonoBehaviour
{
    public UnityEvent<Vector2Int, Map.Node> clicked = new();

    public void setInfo(Vector2Int pos, Map.Node type) {
        position_ = pos;
        type_ = type;
        updateVisuals();
    }

    /// <summary>
    /// Вызывается при необходимости обновить внешний вид кнопки
    /// </summary>
    private void updateVisuals() {
        Sprite icon = null;
        Color col = Color.white;
        Vector2 scale = new(1, 1);

        switch (type_) {
            case Map.Node.REGULAR_ENEMY:
                icon = RegularEnemyIcon;
                break;
            case Map.Node.EVENT:
                icon = EventIcon;
                break;
            case Map.Node.SHOP:
                icon = ShopIcon;
                break;
            case Map.Node.TREASURE:
                icon = TreasureIcon;
                break;
            case Map.Node.REST:
                icon = RestIcon;
                break;
            case Map.Node.ELITE_ENEMY:
                icon = EliteEnemyIcon;
                break;
            case Map.Node.START:
                icon = StartIcon;
                scale *= 2f;
                break;
            case Map.Node.BOSS:
                icon = BossIcon;
                scale *= 2f;
                break;
            case Map.Node.BLACK_MARKET:
                icon = BlackMarketIcon;
                break;
        }

        switch (selection_status_) {
            case SelectionStatus.Current:
                scale *= 1.2f;
                break;
            case SelectionStatus.Inactive:
                col = Color.darkGray;
                break;
            case SelectionStatus.Selected:
                scale *= 1.2f;
                col = Color.green;
                break;
            case SelectionStatus.Selectable:
                col = Color.green;
                break;
        }

        setButton();

        button.GetComponent<Image>().sprite = icon;
        button.GetComponent<Image>().color = col;
        button.GetComponent<RectTransform>().localScale = scale;
    }

    public void Start() {
        setButton();
        button.onClick.AddListener(
            () => clicked.Invoke(position_, type_)
        );
    }

    private void setButton() =>
        button ??= transform.Find("Button").GetComponent<Button>();

    /// <summary>
    /// Размер кнопки в обычном состоянии (используется ExpeditionMapView для рассчета позиций)
    /// </summary>
    public static Vector2 size => new(50, 50);

    public Button button { get; private set; } = null;
    public Vector2Int position => position_;
    [SerializeField] private Vector2Int position_;
    [SerializeField] private Map.Node type_;

    public SelectionStatus selectionStatus {
        get => selection_status_;
        set {
            selection_status_ = value;
            button.interactable =
                selection_status_ == SelectionStatus.Selectable;
            updateVisuals();
        }
    }
    private SelectionStatus selection_status_;

    [SerializeField] private Sprite RegularEnemyIcon;
    [SerializeField] private Sprite EventIcon;
    [SerializeField] private Sprite ShopIcon;
    [SerializeField] private Sprite RestIcon;
    [SerializeField] private Sprite EliteEnemyIcon;
    [SerializeField] private Sprite TreasureIcon;
    [SerializeField] private Sprite StartIcon;
    [SerializeField] private Sprite BossIcon;
    [SerializeField] private Sprite BlackMarketIcon;

    public enum SelectionStatus {
        Normal,
        Inactive,
        Current,
        Selectable,
        Selected
    }
}
