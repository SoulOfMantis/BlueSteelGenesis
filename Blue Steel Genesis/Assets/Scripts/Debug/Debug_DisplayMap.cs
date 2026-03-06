using UnityEngine;

public class Debug_DisplayMap : MonoBehaviour
{
    void Start()
    {
        if (map_view != null)
            GameState.Run.Expedition.displayMap(map_view.GetComponent<ExpeditionMapView>());
    }

    [SerializeField]
    GameObject map_view;
}
