using System;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine.Group;
using FFXIVClientStructs.FFXIV.Client.LayoutEngine.Node;
using FFXIVClientStructs.Interop;
using FFXIVClientStructs.STD;

namespace ReMakePlacePlugin.Preview;

/// <summary>
/// Helper struct to hold data on real furniture when we go to re-enable them.
/// This is to fix certain models having the wrong models enabled after previewing.
/// </summary>
public struct FurnitureState
{
    public HousingObjectId Id;

    private readonly bool[] _enabledInstances;

    public unsafe FurnitureState(Pointer<HousingObject> obj)
    {
        if (obj.IsNull) throw new NullReferenceException("Provided HousingObject is null");
        Id = obj.Value->HousingObjectId;

        SharedGroupLayoutInstance* group = obj.Value->SharedGroupLayoutInstance;
        if (group is null)
        {
            _enabledInstances = [];
            return;
        }

        ref StdVector<Pointer<ChildNodeInstance>> instances = ref group->Instances.Instances;
        _enabledInstances = new bool[instances.Count];

        for (int i = 0; i < instances.Count; i++)
        {
            Pointer<ChildNodeInstance> ptr = instances[i];
            if (ptr.IsNull) continue;
            if (!ptr.Value->Instance->IsActive) continue;
            _enabledInstances[i] = true;
        }
    }

    public unsafe void ApplyStateTo(Pointer<HousingObject> obj)
    {
        if (obj.IsNull) throw new NullReferenceException("Provided HousingObject is null");
        
        SharedGroupLayoutInstance* group = obj.Value->SharedGroupLayoutInstance;
        if (group is null) return;

        ref StdVector<Pointer<ChildNodeInstance>> instances = ref group->Instances.Instances;
        for (int i = 0; i < instances.Count; i++)
        {
            Pointer<ChildNodeInstance> ptr = instances[i];
            if (ptr.IsNull) continue;
            ptr.Value->Instance->SetActive(_enabledInstances[i]);
        }
    }
}