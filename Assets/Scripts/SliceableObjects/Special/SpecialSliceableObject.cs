using System.Collections;
using UnityEngine;

public class SpecialSliceableObject : SliceableObject
{
    private SpecialObjectModule effectModule;

    public override void Initialize(SliceableObjectData data, Vector2 targetPos)
    {
        base.Initialize(data, targetPos);
        effectModule = GetComponent<SpecialObjectModule>();
    }

    protected override void SliceActions()
    {
        Effects(false);
        effectModule.UsePowerUp();
    }
}