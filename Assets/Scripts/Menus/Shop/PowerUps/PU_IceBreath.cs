using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_IceBreath : PowerUp
{
    private List<SliceableObject> sliceableObjectCache = new List<SliceableObject>();
    protected override void Use()
    {
        ObjectLauncher.OnObjectsLaunched += OnObjectsLaunched;
        PowerUpManager.MakedVignette();
    }

    private void OnObjectsLaunched(List<SliceableObject> sliceableObjects)
    {
        for (int i = 0; i < sliceableObjects.Count; i++)
        {
            sliceableObjects[i].Freeze();
        }
        ObjectLauncher.SuccessfullSlice = true;
        sliceableObjectCache.AddRange(sliceableObjects);
    }

    protected override void Deactivate()
    {
        ObjectLauncher.OnObjectsLaunched -= OnObjectsLaunched;
        for (int i = 0; i < sliceableObjectCache.Count; i++)
        {
            if (sliceableObjectCache[i] != null)
                sliceableObjectCache[i].UnFreeze();
        }

        sliceableObjectCache.Clear();
        PowerUpManager.CloseVignette();
    }
}
