using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : AdContainer
{
    protected Action onRewardAdWatched;
    protected Action onRewardAdCancelled;
    protected Action onAdLoaded;

    public void Setup(Action onAdLoaded)
    {
        this.onAdLoaded += onAdLoaded;
        Advertisement.Load(id, adsManager);
    }
    public void Show(Action onRewardAdWatched, Action onRewardAdCancelled)
    {
        this.onRewardAdWatched += onRewardAdWatched;
        this.onRewardAdCancelled += onRewardAdCancelled;
        base.OnAdLoaded();
    }

    public override void OnAdLoaded()
    {
        onAdLoaded?.Invoke();
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

    protected override void CleanUp()
    {
        onRewardAdWatched = null;
        onAdLoaded = null;
    }
}
