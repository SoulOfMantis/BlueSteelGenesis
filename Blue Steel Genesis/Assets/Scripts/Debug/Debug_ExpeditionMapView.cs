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

    [ContextMenu("Debug: make graph")]
    public void build() {
        clear();

        var view = getView();
        var map = ExpeditionMap.generate(
            width, height,
            ArrayUtil.fromHexString(global_seed),
            new() { id = biome_id, missing_node_rate = missing_node_rate },
            biome_stage, lives_left, parts_info);

        view.make(map);



        biome_seed_obj.GetComponent<TMPro.TMP_Text>().text = "Biome seed: " + ArrayUtil.toHexString(BitConverter.GetBytes(map.biome_seed));
        local_seed_obj.GetComponent<TMPro.TMP_Text>().text = "Local seed: " + ArrayUtil.toHexString(BitConverter.GetBytes(map.local_seed));
        if (Application.isPlaying)
            view.currentNode = map.upside_down ? new(-1, map.height) : new(-1, -1);
    }

    [ContextMenu("Debug: clear")]
    public void clear() {
        getView().clear();
        biome_seed_obj.GetComponent<TMPro.TMP_Text>().text = string.Empty;
        local_seed_obj.GetComponent<TMPro.TMP_Text>().text = string.Empty;
    }

    private void Start() {
        build();
    }

    ExpeditionMapView getView() =>
        transform.Find("ExpeditionMapView").GetComponent<ExpeditionMapView>();

    public GameObject biome_seed_obj;
    public GameObject local_seed_obj;
}
