using System.Collections;
using UnityEngine;

public class DoubleCoinSliceableObject : SpecialObjectModule
{
    public override void UsePowerUp()
    {
        CameraShake.Shake(ShakeIntensity.Low);
        PlayerManager.AddStatusEffect(StatusEffect.DoubleCoin, effectDuration);
    }
}