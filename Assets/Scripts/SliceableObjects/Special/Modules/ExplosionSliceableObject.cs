using System.Collections;
using UnityEngine;

public class ExplosionSliceableObject : SpecialObjectModule
{
    public override void UsePowerUp()
    {
        CameraShake.Shake(ShakeIntensity.Ultra);
        int sliceCount = 0;
        int coin = 0;
        foreach (SliceableObject sliceableObject in ObjectLauncher.LaunchedSliceableObject)
        {
            if (sliceableObject == null || sliceableObject.gameObject == gameObject)
                continue;

            coin += sliceableObject.Coin;
            sliceableObject.Slice();
            sliceCount++;
        }
        PlayerManager.GoodSlice(sliceCount, coin);
    }
}