using ClumsyWizard.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public abstract class AdContainer : Persistant<AdContainer>
{
    protected string id;
    protected AdsManager adsManager;

    public void Initialize(string id, AdsManager adsManager)
    {
        this.id = id;
        this.adsManager = adsManager;
    }

    public virtual void OnAdLoaded()
    {
        Advertisement.Show(id, adsManager);
    }

    public abstract void OnAdWatched(UnityAdsShowCompletionState showCompletionState);
    public abstract void OnAdClick();
    public abstract void OnAdShow();
}