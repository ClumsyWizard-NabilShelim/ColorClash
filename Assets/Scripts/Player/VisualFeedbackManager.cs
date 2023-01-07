using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FeedbackType
{
    CoinEarned,
    Health,
    CoinMultiplier,
    Missed,
    CoinRush,
    Time
}

public class VisualFeedbackManager : MonoBehaviour
{
    [Header("Feedback")]
    [SerializeField] private ClumsyDictionary<FeedbackType, VisualFeedback> visualFeedbacks;
    private VisualFeedback currentFeedback;
    private List<SliceableObject> slicedObjectCache = new List<SliceableObject>();

    private void Awake()
    {
        ObjectSlicer.OnObjectsSliced((List<SliceableObject> slicedObjects) =>
        {
            slicedObjectCache = slicedObjects;
        });
    }

    public void Feedback(FeedbackType type, float amount, float duration)
    {
        if (currentFeedback != null && !visualFeedbacks[type].LowPriority && currentFeedback != visualFeedbacks[type])
        {
            currentFeedback.Close();
            currentFeedback = null;
        }

        visualFeedbacks[type].gameObject.SetActive(true);
        switch (type)
        {
            case FeedbackType.CoinEarned:
                if (amount > 0)
                {
                    Bounds bounds = new Bounds();
                    for (int i = 0; i < slicedObjectCache.Count; i++)
                    {
                        bounds.Encapsulate(slicedObjectCache[i].transform.position);
                    }
                    visualFeedbacks[type].transform.position = bounds.center;
                    visualFeedbacks[type].Show($"+{amount}<sprite=4>", duration);
                }
                break;
            case FeedbackType.Health:
                visualFeedbacks[type].Show($"{(amount > 0 ? '+' : '-')}{Mathf.Abs(amount)}<sprite=3>", duration);
                break;
            case FeedbackType.CoinMultiplier:
                currentFeedback = visualFeedbacks[type];
                currentFeedback.Show($"{amount}x<sprite=4>", duration);
                break;
            case FeedbackType.Missed:
                currentFeedback = visualFeedbacks[type];
                currentFeedback.Show($"Missed!\n-{amount}<sprite=5>", duration);
                break;
            case FeedbackType.CoinRush:
                visualFeedbacks[type].Show($"Coin <sprite=4> Rush!", duration);
                break;
            case FeedbackType.Time:
                visualFeedbacks[type].Show($"{(amount > 0 ? '+' : '-')}{Mathf.Abs(amount)}<sprite=5>", duration);
                break;
            default:
                break;
        }
    }
}