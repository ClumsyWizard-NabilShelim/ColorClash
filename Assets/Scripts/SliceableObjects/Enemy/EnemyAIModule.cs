using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAIModule : MonoBehaviour
{
    public abstract string FeedbackText { get; }
    public abstract void Activate();
}
