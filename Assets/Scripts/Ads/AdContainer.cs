using ClumsyWizard.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public abstract class AdContainer : Persistant<AdContainer>
{
    protected string id;
    protected AdsManager adsManager;
    protected Action onAdLoaded;

    public void Initialize(string id, AdsManager adsManager)
    {
        this.id = id;
        this.adsManager = adsManager;
    }

    public void Setup(Action onAdLoaded)
    {
        this.onAdLoaded += onAdLoaded;
        Advertisement.Load(id, adsManager);
    }

    public void OnAdLoaded()
    {
        onAdLoaded?.Invoke();
    }

    public void Show()
    {
        Debug.Log("base");
        Advertisement.Show(id, adsManager);
    }

    public abstract void OnAdFailedToLoad();
    public abstract void OnAdWatched(UnityAdsShowCompletionState showCompletionState);
    public abstract void OnAdClick();
    public abstract void OnAdShow();

    protected override void CleanUp()
    {
        onAdLoaded = null;
    }
}