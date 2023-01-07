using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Alien : EnemyAIModule
{
    [SerializeField] private int timeDecreaseAmount;
    public override string FeedbackText { get => $"-{timeDecreaseAmount}<sprite=5>"; }

    public override void Activate()
    {
        PlayerManager.DecreaseTime(timeDecreaseAmount);
    }
}
