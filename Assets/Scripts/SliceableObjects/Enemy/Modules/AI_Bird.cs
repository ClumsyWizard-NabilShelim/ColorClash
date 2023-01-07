using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Bird : EnemyAIModule
{
    [SerializeField] private GameObject blockEffectPrefab;
    public override string FeedbackText { get => ""; }

    public override void Activate()
    {
        ScreenBlocker.Block(blockEffectPrefab);
    }
}
