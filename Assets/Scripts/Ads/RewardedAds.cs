using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : AdContainer
{
    protected Action onRewardAdWatched;
    protected Action onRewardAdCancelled;

    public void Show(Action onRewardAdWatched, Action onRewardAdCancelled)
    {
        this.onRewardAdWatched += onRewardAdWatched;
        this.onRewardAdCancelled += onRewardAdCancelled;
        Show();
    }

    public override void OnAdWatched(UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            onRewardAdWatched?.Invoke();
        else if (showCompletionState == UnityAdsShowCompletionState.SKIPPED)
            onRewardAdCancelled?.Invoke();
    }

    public override void OnAdClick() { }
    public override void OnAdShow() { }
    public override void OnAdFailedToLoad()
    {
        onRewardAdCancelled?.Invoke();
    }

    protected override void CleanUp()
    {
        base.CleanUp();
        onRewardAdWatched = null;
        onRewardAdCancelled = null;
    }
}
