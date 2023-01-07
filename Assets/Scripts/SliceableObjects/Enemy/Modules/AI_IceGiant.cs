using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_IceGiant : EnemyAIModule
{
    [SerializeField] private int effectDuration;
    public override string FeedbackText { get => ""; }
    public override void Activate()
    {
        PlayerManager.AddStatusEffect(StatusEffect.CoinFreeze, effectDuration);
        PlayerManager.AddStatusEffect(StatusEffect.PowerUpFreeze, effectDuration);
    }
}
