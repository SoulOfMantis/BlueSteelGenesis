using UnityEngine;
using UnityEngine.UI;

public class ExpeditionMapAutoscroll : MonoBehaviour
{
    void Start()
    {
        if (scrollbar_ == null)
            return;
        int node_cnt = Map.ExpeditionMap.height + (GameState.Run.Expedition.Map.upside_down ? 3 : 2);
        int cur_node = GameState.Run.Expedition.VisitedNodeCount;

        float progress;
        if (cur_node <= 4)
            progress = 0;
        else if (cur_node >= node_cnt - 4)
            progress = 1;
        else
            progress = (float)(cur_node - 4) / (node_cnt - 8);

        scrollbar_.value = GameState.Run.Expedition.Map.upside_down ?
            progress : 1 - progress;
    }

    [SerializeField] private Scrollbar scrollbar_;
}
