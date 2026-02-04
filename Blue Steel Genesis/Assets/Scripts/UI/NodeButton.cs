using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NodeButton : MonoBehaviour
{
    public UnityEvent<Vector2Int, Map.Node> clicked;

    public void setInfo(Vector2Int pos, Map.Node type) {
        position_ = pos;
        type_ = type;
        updateVisuals();
    }

    private void updateVisuals() {
        Color col = Color.white;
        switch (type_) {
            case Map.Node.REGULAR_ENEMY:
                col = Color.gray;
                break;
            case Map.Node.EVENT:
                col = Color.lightCyan;
                break;
            case Map.Node.SHOP:
                col = Color.purple;
                break;
            case Map.Node.TREASURE:
                col = Color.gold;
                break;
            case Map.Node.REST:
                col = Color.lightGreen;
                break;
            case Map.Node.ELITE_ENEMY:
                col = Color.softRed;
                break;
            case Map.Node.START:
                col = Color.blue;
                break;
            case Map.Node.BOSS:
                col = Color.red;
                break;
        }

        setButton();
        button.GetComponent<Image>().color = col;
    }

    [ExecuteAlways]
    public void Start() {
        setButton();
        if (Application.isPlaying)
            button.onClick.AddListener(
                () => clicked.Invoke(position_, type_)
            );
    }

    private void setButton() =>
        button = transform.Find("Button").GetComponent<Button>();

    public static Vector2 size => new(30, 30);

    public Button button { get; private set; }
    private Vector2Int position_;
    private Map.Node type_;
}
