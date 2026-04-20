using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class EventEffect
{
    public int healthChange;   
    public int moneyChange;   
    public int materialChange;
    public int maxHealthChange;        // изменение максимального здоровья
    public int energyChange;           // изменение текущей энергии
    public int maxEnergyChange;        // изменение максимальной энергии
    public List<GameModule> addModules;   // модули, которые будут добавлены 
    public List<string> removeModuleIds;  // ID модулей для удаления


    /// <summary>
    /// Возвращает строковое описание эффекта для отображения на кнопке.
    /// </summary>
    public string GetDescription()
    {
        var sb = new StringBuilder();
        if (healthChange != 0)
            sb.Append(healthChange > 0 ? $"+{healthChange}" : $"{healthChange}").Append(" HP, ");
        if (maxHealthChange != 0)
            sb.Append(maxHealthChange > 0 ? $"+{maxHealthChange}" : $"{maxHealthChange}").Append(" Max HP, ");
        if (energyChange != 0)
            sb.Append(energyChange > 0 ? $"+{energyChange}" : $"{energyChange}").Append(" Energy, ");
        if (maxEnergyChange != 0)
            sb.Append(maxEnergyChange > 0 ? $"+{maxEnergyChange}" : $"{maxEnergyChange}").Append(" Max Energy, ");
        if (moneyChange != 0)
            sb.Append(moneyChange > 0 ? $"+{moneyChange}" : $"{moneyChange}").Append(" Gold, ");
        if (materialChange != 0)
            sb.Append(materialChange > 0 ? $"+{materialChange}" : $"{materialChange}").Append(" Materials, ");
        if (addModules != null && addModules.Count > 0)
            sb.Append($"Add {addModules.Count} module(s), ");
        if (removeModuleIds != null && removeModuleIds.Count > 0)
            sb.Append($"Remove {removeModuleIds.Count} module(s), ");

        if (sb.Length == 0)
            return "No effect";
        sb.Length -= 2; // убрать последнюю запятую и пробел
        return sb.ToString();
    }
}
