using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.Linq;
using System.Collections.Generic;

public class Debug_ModuleGeneration : MonoBehaviour
{
    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Description;
    [SerializeField] TMP_Text Keywords;
    [SerializeField] TMP_Text DrawnModuleNames;
    List<GameModule> DrawnModules = new();
    public void DrawNextCommonModule()
    {
        var module = GameState.Run.Expedition.ModuleGen.GetNextCommonModule(DrawnModules);
        DrawnModules.Add(module);
        DrawnModuleNames.text += $"{module.Name}, ";
        Name.text = module.Name;
        Description.text = module.Description();
        Keywords.text = "";
        foreach (var k in module.GetKeywords().Where(kw => kw is VisibleKeyword).Select(kw => kw as VisibleKeyword))
            Keywords.text += $"{k.Name}; ";
    }
    public void DrawNextBossModule()
    {
        var module = GameState.Run.Expedition.ModuleGen.GetNextBossModule(DrawnModules);
        DrawnModules.Add(module);
        DrawnModuleNames.text += $"{module.Name}, ";
        Name.text = module.Name;
        Description.text = module.Description();
        Keywords.text = "";
        foreach (var k in module.GetKeywords().Where(kw => kw is VisibleKeyword).Select(kw => kw as VisibleKeyword))
            Keywords.text += $"{k.Name}; ";
    }
}
