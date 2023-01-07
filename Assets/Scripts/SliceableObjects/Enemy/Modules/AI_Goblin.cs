using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Goblin : EnemyAIModule
{
    [SerializeField] private int stealAmount;
    public override string FeedbackText { get => $"-{stealAmount}<sprite=4>"; }

    public override void Activate()
    {
        PlayerManager.RemoveCoin(stealAmount);
    }
}
