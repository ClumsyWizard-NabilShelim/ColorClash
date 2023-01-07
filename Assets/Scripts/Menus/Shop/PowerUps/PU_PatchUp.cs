using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_PatchUp : PowerUp
{
    protected override void Use()
    {
        PlayerManager.Heal((int)Data.GetEffectValue(Rank));
    }

    protected override void Deactivate(){}
}
