using System;
using UnityEngine;

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

    public void make(Map.Node[,] map, bool upside_down)
    {
        if (map == null) return;

        setPanel();

        float node_gap;
        var effective_rect = calculateEffectiveRect(
            new(map.GetLength(1), map.GetLength(0)),
            NodeButton.size,
            out node_gap
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
            button.clicked.AddListener((pos, type) => {
                lastSelection = new(pos, type);
                //TODO: highlight selection
            });
        }
        for (int line = 0; line < map.GetLength(0); ++line)
            for (int x = 0; x < map.GetLength(1); ++x)
                if (map[line, x] != Map.Node.DISABLED)
                    addButton(new Vector2Int(x, line), map[line, x]);
        addButton(new Vector2Int(-1, -1), upside_down ? Map.Node.BOSS : Map.Node.START);
        addButton(new Vector2Int(-1, map.GetLength(0)), upside_down ? Map.Node.START : Map.Node.BOSS);
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
    private GameObject panel;
    [Range(0f, .4f)] public float margin = 0.05f;

    public (Vector2Int pos, Map.Node type)? lastSelection { get; private set; } = null;
}
