using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.Linq;

public class Debug_ModuleGeneration : MonoBehaviour
{
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Description;
    [SerializeField] TMP_Text Keywords;
    public void DrawNextCommonModule()
    {
        var module = GameState.Run.Expedition.ModuleGen.GetNextCommonModule();
        Name.text = module?.Name;
        Description.text = module?.Description();
        Keywords.text = "";
        foreach (var k in module?.Keywords)
            Keywords.text += $"{k.Name}; ";
    }
    public void DrawNextBossModule()
    {
        var module = GameState.Run.Expedition.ModuleGen.GetNextBossModule();
        Name.text = module?.Name;
        Description.text = module?.Description();
        Keywords.text = "";
        foreach (var k in module?.Keywords)
            Keywords.text += $"{k.Name}; ";
    }
}
