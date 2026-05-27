using System.Linq;
using Anyder.Interop;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine;

namespace ReMakePlacePlugin.Util;

public static unsafe class PreviewUtils
{
    /// <summary>
    /// Toggles the current furniture's visibility while retaining player interaction.
    /// </summary>
    /// <param name="enabled">Whether the furniture should be visible</param>
    public static void ToggleFurniture(bool enabled)
    {
        var man = HousingManager.Instance();
        if (man == null) return;

        var furnVector = man->GetFurnitureManager()->FurnitureVector;
        var objArray = man->GetFurnitureManager()->ObjectManager.ObjectArray.Objects;
        foreach (var ptr in furnVector)
        {
            var furn = ptr.Value;
            if (furn == null) continue;
            
            var index = furn->Index;
            var obj = (HousingObject*)objArray[index].Value;
            if (obj == null) continue;

            var group = obj->SharedGroupLayoutInstance;
            if (group == null) continue;
            
            group->SetActive(enabled);
            group->SetColliderActive(true); // cool that this works :D//
        }
    }
}