using System;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionMapView : MonoBehaviour
{
    public void clear() {
        setPanel();
        for (int i = panel.transform.childCount; i-- != 0;)
            if (Application.isEditor)
                DestroyImmediate(panel.transform.GetChild(i).gameObject);
            else
                Destroy(panel.transform.GetChild(i).gameObject);
    }

    public void make(Map.ExpeditionMap map)
    {
        if (map == null) return;
        
        setPanel();
        map_ = map;

        var effective_rect = calculateEffectiveRect(
            new(map_.map.GetLength(1), map_.map.GetLength(0)),
            NodeButton.size,
            out float node_gap
        );

        void addButton(Vector2Int position, Map.Node type) {
            Vector2 local_position = new(
                NodeButton.size.x/2 + node_gap * position.x,
                NodeButton.size.y/2 + node_gap * (position.y + 1));
            local_position.y = effective_rect.height - local_position.y;

            if (type == Map.Node.START || type == Map.Node.BOSS)
                local_position.x = effective_rect.width / 2;

            var button_obj = Instantiate(button_prefab, panel.transform);
            button_obj.transform.localPosition = effective_rect.position + local_position;

            var button = button_obj.GetComponent<NodeButton>();
            button.setInfo(position, type);
            getButtonRef(position) = button;
        }
        buttons_ = new NodeButton[map_.map.GetLength(0), map_.map.GetLength(1)];
        for (int line = 0; line < map_.map.GetLength(0); ++line)
            for (int x = 0; x < map_.map.GetLength(1); ++x)
                if (map_.map[line, x] != Map.Node.DISABLED)
                    addButton(new Vector2Int(x, line), map_.map[line, x]);
        addButton(new Vector2Int(-1, -1), map.upside_down ? Map.Node.BOSS : Map.Node.START);
        addButton(new Vector2Int(-1, map_.map.GetLength(0)), map.upside_down ? Map.Node.START : Map.Node.BOSS);

        connectButtons();
    }

    private void Start() {
        setPanel();
        confirmSelectionButton = confirm_selection_button_;
    }

    private void connectButtons() {
        foreach (Transform button_transform in panel.transform) {
            var button = button_transform.gameObject.GetComponent<NodeButton>();
            
            button.clicked.AddListener((pos, type) => {
                lastSelection = new(pos, type);
            });
        }
    }

    private void updateSelectionStatus() {
        resetSelectionStatus();
        getButton(currentNode).selectionStatus = NodeButton.SelectionStatus.Current;
        foreach (Vector2Int target in map_.listTargets(currentNode))
            getButton(target).selectionStatus = NodeButton.SelectionStatus.Selectable;
    }
    private void resetSelectionStatus() {
        for (int line = 0; line < buttons_.GetLength(0); ++line)
            for (int x = 0; x < buttons_.GetLength(1); ++x)
                if (buttons_[line, x] != null)
                    buttons_[line, x].selectionStatus = NodeButton.SelectionStatus.Normal;
        upper_end_button_.selectionStatus = NodeButton.SelectionStatus.Normal;
        lower_end_button_.selectionStatus = NodeButton.SelectionStatus.Normal;
    }
    public void confirmSelection() {
        if (last_selection_ == null)
            return;
        triggerSubsystem();
        currentNode = last_selection_.Value.pos;
    }
    private void triggerSubsystem() {
        Debug.Log($"Player selected: {last_selection_?.type} at {last_selection_?.pos}");
        //TODO: implement
    }

    private NodeButton getButton(Vector2Int pos) {
        if (pos.x == -1) {
            if (pos.y == -1)
                return upper_end_button_;
            if (pos.y == map_.map.GetLength(0))
                return lower_end_button_;
            throw new ArgumentOutOfRangeException();
        }
        return buttons_[pos.y, pos.x];
    }
    private ref NodeButton getButtonRef(Vector2Int pos) {
        if (pos.x == -1) {
            if (pos.y == -1)
                return ref upper_end_button_;
            if (pos.y == map_.map.GetLength(0))
                return ref lower_end_button_;
            throw new ArgumentOutOfRangeException();
        }
        return ref buttons_[pos.y, pos.x];
    }

    private Rect calculateEffectiveRect(Vector2Int graph_dimensions, Vector2 node_button_size, out float node_gap) {
        Rect full_rect =
            (transform.parent is RectTransform parent_transform) ? parent_transform.rect : new();

        Rect effective_rect = new();
        effective_rect.x = full_rect.width * margin;
        effective_rect.width = full_rect.width - 2 * effective_rect.x;
        effective_rect.y = full_rect.height * margin;
        effective_rect.height = full_rect.height - 2 * effective_rect.y;

        float node_gap_by_width = (effective_rect.width - node_button_size.x) / (graph_dimensions.x - 1);
        float node_gap_by_height = (effective_rect.height - node_button_size.y) / (graph_dimensions.y + 1);
        node_gap = Mathf.Min(node_gap_by_width, node_gap_by_height);

        float expected_graph_width = node_gap * (graph_dimensions.x - 1) + node_button_size.x;
        float expected_graph_height = node_gap * (graph_dimensions.y + 1) + node_button_size.y;
        effective_rect.position = new Vector2(
            (full_rect.width - expected_graph_width) / 2,
            (full_rect.height - expected_graph_height) / 2
        );
        effective_rect.size = new Vector2(expected_graph_width, expected_graph_height);

        return effective_rect;
    }

    private void setPanel() =>
        panel = transform.Find("Panel").gameObject;

    public GameObject button_prefab;
    private GameObject panel = null;
    [Range(0f, .4f)] public float margin = 0.05f;

    private Map.ExpeditionMap map_;
    private NodeButton[,] buttons_;
    private NodeButton upper_end_button_, lower_end_button_;

    public Button confirmSelectionButton {
        get => confirm_selection_button_;
        set {
            if (confirm_selection_button_ != null) {
                confirm_selection_button_.onClick.RemoveListener(confirmSelection);
            }
            confirm_selection_button_ = value;
            confirm_selection_button_?.onClick.AddListener(confirmSelection);
        }
    }
    [SerializeField]
    private Button confirm_selection_button_;

    public (Vector2Int pos, Map.Node type)? lastSelection {
        get => last_selection_;
        private set {
            if (last_selection_ != null)
                getButton(last_selection_.Value.pos).selectionStatus = NodeButton.SelectionStatus.Selectable;
            if (value != null)
                getButton(value.Value.pos).selectionStatus = NodeButton.SelectionStatus.Selected;

            last_selection_ = value;
            if (confirm_selection_button_ != null)
                confirm_selection_button_.interactable = last_selection_ != null;
        }
    }
    private (Vector2Int pos, Map.Node type)? last_selection_ = null;

    public Vector2Int currentNode {
        get => current_node_;
        set {
            current_node_ = value;
            lastSelection = null;
            updateSelectionStatus();
        }
    }
    private Vector2Int current_node_;
}
