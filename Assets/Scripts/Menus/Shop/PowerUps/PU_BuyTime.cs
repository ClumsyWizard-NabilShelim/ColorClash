using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_BuyTime : PowerUp
{
    protected override void Use()
    {
        PlayerManager.AddTime(Data.GetEffectValue(Rank));
    }

    protected override void Deactivate() { }
}
