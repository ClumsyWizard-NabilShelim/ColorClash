using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_FireDemon : EnemyAIModule
{
    [SerializeField] private int effectDuration;
    public override string FeedbackText { get => ""; }

    public override void Activate()
    {
        PlayerManager.AddStatusEffect(StatusEffect.Burn, effectDuration);
    }
}
