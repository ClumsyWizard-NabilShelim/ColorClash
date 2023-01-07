using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_ColorBomb : PowerUp
{
    protected override void Use()
    {
        ObjectLauncher.OnObjectsLaunched += OnObjectsLaunched;
    }

    private void OnObjectsLaunched(List<SliceableObject> sliceableObjects)
    {
        ObjectColor color = (ObjectColor)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ObjectColor)).Length);
        for (int i = 0; i < sliceableObjects.Count; i++)
        {
            if (sliceableObjects[i].Type != SliceableObjectType.Color)
                continue;

            ((ShapeSliceableObject)sliceableObjects[i]).SetColor(color, ObjectLauncher.GetShapeData(color));
        }
    }

    protected override void Deactivate()
    {
        ObjectLauncher.OnObjectsLaunched -= OnObjectsLaunched;
    }
}
