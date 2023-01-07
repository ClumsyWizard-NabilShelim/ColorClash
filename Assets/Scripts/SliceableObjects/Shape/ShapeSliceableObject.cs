using System.Collections;
using UnityEngine;

public class ShapeSliceableObject : SliceableObject
{
    [field: Header("Stats")]
    public ObjectColor Color { get; private set; }

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer gfx;

    public void Initialize(ObjectColor objectColor, ShapeSliceableObjectData data, Vector2 targetPos)
    {
        SetColor(objectColor, data);
        base.Initialize(data, targetPos);
    }

    public void SetColor(ObjectColor color, ShapeSliceableObjectData data)
    {
        Color = color;

        gfx.sprite = data.Sprite;
        gfx.color = data.Color;
    }

    protected override void SliceActions()
    {
        CameraShake.Shake(ShakeIntensity.Low);
        Effects(true, gfx.color);
    }
}