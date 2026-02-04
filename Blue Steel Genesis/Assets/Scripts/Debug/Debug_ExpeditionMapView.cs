using Map;
using System;
using UnityEngine;

public class Debug_ExpeditionMapView : MonoBehaviour
{
    [Range(3, 16)] public uint width = 4;
    [Range(3, 16)] public uint height = 7;
    public string global_seed = "f1f2f3f4";
    public uint biome_id = 1;
    [Range(0f, 1f)] public float missing_node_rate = .5f;
    [Range(0, 5)] public uint biome_stage = 1;
    [Range(0, 5)] public uint lives_left = 2;
    public byte[] parts_info = new byte[5]{0x1d, 0x2c, 0x3b, 0x4a, 0x59};
    public bool upside_down = false;

    [ContextMenu("Debug: make graph")]
    public void build() {
        clear();

        var map_builder = GetComponent<ExpeditionMap>();
        var view = getView();
        map_builder.generate(
            width, height,
            ArrayUtil.fromHexString(global_seed),
            new() { id = biome_id, missing_node_rate = missing_node_rate },
            biome_stage, lives_left, parts_info);

        view.make(map_builder.map, upside_down);
    }

    [ContextMenu("Debug: clear")]
    public void clear() {
        getView().clear();
    }

    ExpeditionMapView getView() =>
        transform.Find("ExpeditionMapView").GetComponent<ExpeditionMapView>();
}
