using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFreezeSliceableObject : SpecialObjectModule
{
    public override void UsePowerUp()
    {
        CameraShake.Shake(ShakeIntensity.Low);
        PlayerManager.AddStatusEffect(StatusEffect.TimeFreeze, effectDuration);
    }
}
