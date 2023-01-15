using ClumsyWizard.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public enum AdType
{
    Interstitial,
    Rewareded,
    Banner
}

public enum AdsSaveTags
{
    LevelChangeCoolDown
}

public class AdsManager : Persistant<AdsManager>, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string gameID;
    private bool testMode;

    private bool isInitialized;
    private Action onInitialied;

    [SerializeField] private ClumsyDictionary<string, AdType> adIDs;
    [SerializeField] private ClumsyDictionary<AdType, AdContainer> ads;

    [Header("Ads Control")]
    [SerializeField] private int interstitialADCooldown;
    public static int InterstitialADCooldown { get => Instance.interstitialADCooldown; }

    protected override void Awake()
    {
        base.Awake();

        #if UNITY_EDITOR
             testMode = true;
#else
            if(Application.platform == RuntimePlatform.Android)
                testMode = false;
            else
                testMode = true; 
#endif

        Advertisement.Initialize(gameID, testMode, this);

        foreach (AdType type in ads.Keys)
        {
            ads[type].Initialize(adIDs.GetKeyByValue(type), this);
        }

        SaveLoadManager.SetInt(AdsSaveTags.LevelChangeCoolDown.ToString(), interstitialADCooldown);

        SceneManagement.OnNewSceneLoadedCore += () =>
        {
            if (SceneManagement.isGameScene)
                ((BannerAds)ads[AdType.Banner]).Hide();
            else
                ((BannerAds)ads[AdType.Banner]).Show();
        };
    }

    //Initialization
    public void OnInitializationComplete()
    {
        isInitialized = true;
        onInitialied?.Invoke();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        isInitialized = false;
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }

    //Loading
    public void OnUnityAdsAdLoaded(string placementId)
    {
        ads[adIDs[placementId]].OnAdLoaded();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        ads[adIDs[placementId]].OnAdFailedToLoad();
        Debug.Log($"Error loading Ad Unit: {placementId} - {error} - {message}");
    }

    //Showing
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {placementId}: {error} - {message}");
    }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Complete");
        ads[adIDs[placementId]].OnAdWatched(showCompletionState);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Start");
        ads[adIDs[placementId]].OnAdShow();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Click");
        ads[adIDs[placementId]].OnAdClick();
    }

    //Getters
    public static void GetAd<T>(AdType type, Action<T> callback) where T : AdContainer
    {
        if (!Instance.isInitialized)
        {
            Instance.onInitialied += () => GetAd(type, callback);
        }
        else
        {
            callback((T)Instance.ads[type]);
        }
    }

    //Misc
    protected override void CleanUp()
    {
    }
}
