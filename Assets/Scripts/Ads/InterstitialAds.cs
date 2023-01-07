using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : AdContainer
{
    protected Action onAdDisplayed;

    public void Show(Action onAdDisplayed)
    {
        this.onAdDisplayed += onAdDisplayed;
        Advertisement.Load(id, adsManager);
    }

    public override void OnAdWatched(UnityAdsShowCompletionState showCompletionState) { }

    public override void OnAdClick() { }

    public override void OnAdShow()
    {
        onAdDisplayed?.Invoke();
    }

    protected override void CleanUp()
    {
        onAdDisplayed = null;
    }
}
