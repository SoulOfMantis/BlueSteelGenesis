using Map;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionMapView : MonoBehaviour
{
    public void clear() {
        initUIComponentRefs();
        Action<GameObject> destroy = Application.isEditor ?
            o => DestroyImmediate(o) :
            o => Destroy(o);

        for (int i = panel.transform.childCount; i-- != 0;)
            if (panel.transform.GetChild(i).GetComponent<MultilineRenderer2D>() is MultilineRenderer2D renderer)
                renderer.clear();
            else
                destroy(panel.transform.GetChild(i).gameObject);
    }

    /// <summary>
    /// Создает отображаемую карту
    /// </summary>
    /// <param name="progress_info">Данные о прохождении (объект будет обновлен)</param>
    public void make(ExpeditionMap map, ExpeditionMapProgressInfo progress_info = null)
    {
        if (map == null) return;
        progress_info ??= new(map);
        
        initUIComponentRefs();
        map_ = map;
        progress_ = progress_info;

        var effective_rect = calculateEffectiveRect(
            new(ExpeditionMap.width, ExpeditionMap.height),
            NodeButton.size,
            map_.upside_down,
            out float node_gap
        );

        void addButton(Vector2Int position, Node type) {
            Vector2 local_position = new(
                NodeButton.size.x/2 + node_gap * position.x,
                NodeButton.size.y/2 + node_gap *
                    (position.y + (map_.upside_down ? 2 : 1)));
            local_position.y = effective_rect.height - local_position.y;

            if (type == Node.START || type == Node.BOSS || type == Node.BLACK_MARKET)
                local_position.x = effective_rect.width / 2;

            var button_obj = Instantiate(button_prefab, panel.transform);
            button_obj.transform.localPosition = effective_rect.position + local_position;

            var button = button_obj.GetComponent<NodeButton>();
            button.setInfo(position, type);
            getButtonRef(position) = button;
        }
        buttons_ = new NodeButton[ExpeditionMap.height, ExpeditionMap.width];
        for (int line = 0; line < ExpeditionMap.height; ++line)
            for (int x = 0; x < ExpeditionMap.width; ++x)
                if (map_.map[line, x] != Node.DISABLED)
                    addButton(new Vector2Int(x, line), map_.map[line, x]);
        addButton(map.start_node_pos, Node.START);
        addButton(map.boss_node_pos, Node.BOSS);
        if (map_.upside_down)
            addButton(map.black_market_node, Node.BLACK_MARKET);

        void linkButtons(NodeButton b1, NodeButton b2) {
            var line = new MultilineRenderer2D.Line() {
                from = b1.GetComponent<Transform>().localPosition,
                to = b2.GetComponent<Transform>().localPosition,
                color = Color.darkCyan,
                width = 6
            };
            line_renderer.addLine(b1.position, b2.position, line);
        }

        foreach (var button in  buttons_) if (button)
            foreach (var target in map.listTargets(button.position))
                linkButtons(button, getButton(target));
        foreach (var target in map.listTargets(map_.start_node_pos))
            linkButtons(getButton(map_.start_node_pos), getButton(target));
        foreach (var target in map.listTargets(map_.boss_node_pos))
            linkButtons(getButton(map_.boss_node_pos), getButton(target));

        foreach (Transform button_transform in panel.transform) {
            var button = button_transform.gameObject.GetComponent<NodeButton>();
            if (!button) continue;

            button.clicked.AddListener((pos, type) => {
                lastSelection = new(pos, type);
            });
        }

        updateSelectionStatus();
        updateNodeLinks();
    }

    private void Start() {
        initUIComponentRefs();
        confirmSelectionButton = confirm_selection_button_;
    }

    private void updateNodeLinks() {
        foreach (var src_node in Enumerable.Repeat(map_.start_node_pos, 1).Concat(map_.listReachable(map_.start_node_pos)))
            foreach (var target in map_.listTargets(src_node)) {
                NodeButton.SelectionStatus src_selection_status = getButton(src_node).selectionStatus,
                                           target_selection_status = getButton(target).selectionStatus;
                MultilineRenderer2D.Line.Type line_type;

                if (src_selection_status == NodeButton.SelectionStatus.Inactive ||
                    target_selection_status == NodeButton.SelectionStatus.Inactive)
                    line_type = MultilineRenderer2D.Line.Type.INACTIVE;
                else if (target_selection_status == NodeButton.SelectionStatus.Selectable ||
                         target_selection_status == NodeButton.SelectionStatus.Selected)
                    line_type = MultilineRenderer2D.Line.Type.NORMAL;
                else
                    line_type = MultilineRenderer2D.Line.Type.DASHED;
                
                line_renderer.updateLineType(src_node, target, line_type);
            }

        foreach (var path_link in progress_.pathLinks())
            line_renderer.updateLineType(path_link.Item1, path_link.Item2, MultilineRenderer2D.Line.Type.NORMAL);
    }

    private void updateSelectionStatus() {
        resetSelectionStatus();

        foreach (var path_node in progress_.path)
            getButton(path_node).selectionStatus = NodeButton.SelectionStatus.Normal;
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
        switch (last_selection_?.type)
        {
            case Node.TREASURE:
                GameState.Run.Expedition.TreasureSubsystem.Trigger();
                break;
            case Node.REGULAR_ENEMY:
                GameState.Run.Expedition.CombatSystem.TriggerNormalEncounter();
                break;
            case Node.ELITE_ENEMY:
                GameState.Run.Expedition.CombatSystem.TriggerEliteEncounter();
                break;
            case Node.BOSS:
                GameState.Run.Expedition.CombatSystem.TriggerBossEncounter();
                break;
            case Node.SHOP:
                GameState.Run.Expedition.Shop.TriggerShop();
                break;
            case Node.BLACK_MARKET:
                GameState.Run.Expedition.Shop.TriggerBlackMarket();
                break;
                GameState.Run.Expedition.EventSystem.TriggerEvent();
                break;
            default:
                break;
        }
    }

    private NodeButton getButton(Vector2Int pos) {
        if (pos.x == -1) {
            if (pos.y == -2)
                return black_market_button_;
            if (pos.y == -1)
                return upper_end_button_;
            if (pos.y == ExpeditionMap.height)
                return lower_end_button_;
            throw new ArgumentOutOfRangeException();
        }
        return buttons_[pos.y, pos.x];
    }
    private ref NodeButton getButtonRef(Vector2Int pos) {
        if (pos.x == -1) {
            if (pos.y == -2)
                return ref black_market_button_;
            if (pos.y == -1)
                return ref upper_end_button_;
            if (pos.y == ExpeditionMap.height)
                return ref lower_end_button_;
            throw new ArgumentOutOfRangeException();
        }
        return ref buttons_[pos.y, pos.x];
    }

    /// <summary>
    /// Рассчитывает размеры прямоугольника для размещения карты
    /// </summary>
    /// <param name="graph_dimensions">Размеры карты (кол-во узлов)</param>
    /// <param name="node_button_size">Размеры кнопки</param>
    /// <param name="node_gap">Рекомендуемое расстояние между центрами соседних вершин</param>
    private Rect calculateEffectiveRect(Vector2Int graph_dimensions, Vector2 node_button_size, bool upside_down, out float node_gap) {
        Rect full_rect =
            (transform.parent is RectTransform parent_transform) ? parent_transform.rect : new();

        Rect effective_rect = new();
        effective_rect.x = full_rect.width * margin;
        effective_rect.width = full_rect.width - 2 * effective_rect.x;
        effective_rect.y = full_rect.height * margin;
        effective_rect.height = full_rect.height - 2 * effective_rect.y;

        float node_gap_by_width = (effective_rect.width - node_button_size.x) / (graph_dimensions.x - 1);
        float node_gap_by_height = (effective_rect.height - node_button_size.y) / (graph_dimensions.y + (upside_down ? 2 : 1));
        node_gap = Mathf.Min(node_gap_by_width, node_gap_by_height);

        float expected_graph_width = node_gap * (graph_dimensions.x - 1) + node_button_size.x;
        float expected_graph_height = node_gap * (graph_dimensions.y + (upside_down ? 2 : 1)) + node_button_size.y;
        effective_rect.position = new Vector2(
            (full_rect.width - expected_graph_width) / 2,
            (full_rect.height - expected_graph_height) / 2
        );
        effective_rect.size = new Vector2(expected_graph_width, expected_graph_height);

        return effective_rect;
    }

    /// <summary>
    /// Устанавливает ссылки на дочерние элементы
    /// </summary>
    private void initUIComponentRefs() {
        panel = transform.Find("Panel").gameObject;
        line_renderer = panel.transform.Find("MultilineRenderer").gameObject.GetComponent<MultilineRenderer2D>();
    }

    public GameObject button_prefab;
    private GameObject panel = null;
    private MultilineRenderer2D line_renderer = null;
    [Range(0f, .4f), Tooltip("Отступ с каждой стороны родительского элемента")]
    public float margin = 0.05f;

    private ExpeditionMap map_;
    private NodeButton[,] buttons_;
    private NodeButton upper_end_button_, lower_end_button_, black_market_button_;
    private ExpeditionMapProgressInfo progress_;

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

    public (Vector2Int pos, Node type)? lastSelection {
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
    private (Vector2Int pos, Node type)? last_selection_ = null;

    public Vector2Int currentNode {
        get => progress_.currentNode;
        set {
            if (value != progress_.currentNode) {
                progress_.path.Add(value);
                lastSelection = null;
            }
            updateSelectionStatus();
            updateNodeLinks();
        }
    }
}

public class ExpeditionMapProgressInfo {
    public ExpeditionMapProgressInfo(ExpeditionMap map) {
        path = new(ExpeditionMap.height + 2);
        path.Add(map.start_node_pos);
    }
    
    public IEnumerable<(Vector2Int, Vector2Int)> pathLinks() {
        for (int i = 1; i < path.Count; ++i)
            yield return (path[i - 1], path[i]);
    }
    
    public List<Vector2Int> path;
    public Vector2Int currentNode => path[^1];
}
