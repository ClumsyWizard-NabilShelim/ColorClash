using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_InhumanReflex : PowerUp
{
    protected override void Use()
    {
        GameManager.SlowDownFall();
        PowerUpManager.ClassicVignette();
    }
    protected override void Deactivate()
    {
        GameManager.NormalizeFall();
        PowerUpManager.CloseVignette();
    }
}
