using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// Базовый модуль движения (BMM сокращение)
/// </summary>
public class AcceleratedMovement : ActiveModule
{
    uint burnDamage = 1;
    uint burnDuration = 1;
    public AcceleratedMovement() : base()
    {
        range = 5;
        energyCost = 2;
        //Icon_name = "Module_accelerated_movement";
        AddConstKeywords(new MobilityKeyword(), new CommonKeyword());

    }
    public AcceleratedMovement(uint speed) : this()
    {
        range = speed;
    }
    public override HashSet<ModuleKeyword> renewableKeywords()
    {
        var res = base.renewableKeywords();
        res.Add(new InflictKeyword<BurnModule>(PossibleTargets.AllAdjacent, burnDamage, burnDuration));
        return res;
    }

    public override string Description()
    {
        return $"Move to an unoccupied space within {range} cells.\n" + base.Description();
    }
    public override async Task Effect(Character user, Vector3Int pos)
    {
        await user.move(pos, getCellsInRange(user.Position));
        
        foreach (var item in user.Position.NeighborPositions())
            await user.apply(item, new BurnModule(burnDamage, burnDuration));
    }
    public override List<Vector3Int> getCellsInRange(PositionCollection start) =>
        Navigation.Dijkstra.listReachable(start, p => !Entity.tracker.OutOfBounds(p) && !Entity.tracker.IsOccupied(p), range).Except(start).ToList();
}
