using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : AdContainer
{
    protected Action onAdDisplayed;

    public void Setup(Action onAdLoaded, Action onAdDisplayed)
    {
        base.Setup(onAdLoaded);
        this.onAdDisplayed += onAdDisplayed;
    }

    public override void OnAdWatched(UnityAdsShowCompletionState showCompletionState) { }

    public override void OnAdClick() { }

    public override void OnAdShow()
    {
        onAdDisplayed?.Invoke();
    }
    public override void OnAdFailedToLoad()
    {
        onAdDisplayed?.Invoke();
    }
    protected override void CleanUp()
    {
        base.CleanUp();
        onAdDisplayed = null;
    }
}
