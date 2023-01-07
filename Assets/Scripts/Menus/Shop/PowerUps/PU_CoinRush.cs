using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_CoinRush : PowerUp
{
    protected override void Use()
    {
        PlayerManager.AddStatusEffect(StatusEffect.DoubleCoin, Data.GetDuraction(Rank));
    }
    protected override void Deactivate(){}
}
