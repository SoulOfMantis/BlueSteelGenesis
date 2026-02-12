using System;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionMapView : MonoBehaviour
{
    public void clear() {
        setPanel();
        Action<GameObject> destroy = Application.isEditor ?
            o => DestroyImmediate(o) :
            o => Destroy(o);

        for (int i = panel.transform.childCount; i-- != 0;)
            if (panel.transform.GetChild(i).GetComponent<MultilineRenderer2D>() is MultilineRenderer2D renderer)
                renderer.clear();
            else
                destroy(panel.transform.GetChild(i).gameObject);
    }

    public void make(Map.ExpeditionMap map)
    {
        if (map == null) return;
        
        setPanel();
        map_ = map;

        var effective_rect = calculateEffectiveRect(
            new(map_.width, map_.height),
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
        buttons_ = new NodeButton[map_.height, map_.width];
        for (int line = 0; line < map_.height; ++line)
            for (int x = 0; x < map_.width; ++x)
                if (map_.map[line, x] != Map.Node.DISABLED)
                    addButton(new Vector2Int(x, line), map_.map[line, x]);
        addButton(map.start_node_pos, Map.Node.START);
        addButton(map.boss_node_pos, Map.Node.BOSS);

        var line_renderer = panel.transform.Find("MultilineRenderer").gameObject.GetComponent<MultilineRenderer2D>();
        void linkButtons(NodeButton b1, NodeButton b2) {
            var line = new MultilineRenderer2D.Line() {
                from = b1.GetComponent<Transform>().localPosition,
                to = b2.GetComponent<Transform>().localPosition,
                color = Color.darkCyan,
                width = 6
            };
            line_renderer.addLine(line);
        }

        foreach (var button in  buttons_) if (button)
            foreach (var target in map.listTargets(button.position))
                linkButtons(button, getButton(target));
        foreach (var target in map.listTargets(map_.start_node_pos))
            linkButtons(getButton(map_.start_node_pos), getButton(target));

        foreach (Transform button_transform in panel.transform) {
            var button = button_transform.gameObject.GetComponent<NodeButton>();
            if (!button) continue;

            button.clicked.AddListener((pos, type) => {
                lastSelection = new(pos, type);
            });
        }
    }

    private void Start() {
        setPanel();
        confirmSelectionButton = confirm_selection_button_;
    }

    private void updateSelectionStatus() {
        resetSelectionStatus();
        getButton(currentNode).selectionStatus = NodeButton.SelectionStatus.Current;
        foreach (Vector2Int reachable in map_.listReachable(currentNode))
            getButton(reachable).selectionStatus = NodeButton.SelectionStatus.Normal;
        foreach (Vector2Int target in map_.listTargets(currentNode))
            getButton(target).selectionStatus = NodeButton.SelectionStatus.Selectable;
    }
    private void resetSelectionStatus() {
        for (int line = 0; line < buttons_.GetLength(0); ++line)
            for (int x = 0; x < buttons_.GetLength(1); ++x)
                if (buttons_[line, x] != null)
                    buttons_[line, x].selectionStatus = NodeButton.SelectionStatus.Inactive;
        upper_end_button_.selectionStatus = NodeButton.SelectionStatus.Inactive;
        lower_end_button_.selectionStatus = NodeButton.SelectionStatus.Inactive;
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
            if (pos.y == map_.height)
                return lower_end_button_;
            throw new ArgumentOutOfRangeException();
        }
        return buttons_[pos.y, pos.x];
    }
    private ref NodeButton getButtonRef(Vector2Int pos) {
        if (pos.x == -1) {
            if (pos.y == -1)
                return ref upper_end_button_;
            if (pos.y == map_.height)
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
