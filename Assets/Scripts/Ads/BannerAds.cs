using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : AdContainer
{
    private BannerPosition bannerPosition;

    public void Setup(Action onAdLoaded, BannerPosition bannerPosition)
    {
        this.onAdLoaded += onAdLoaded;
        this.bannerPosition = bannerPosition;
        Advertisement.Banner.SetPosition(this.bannerPosition);

        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnAdLoaded,
            errorCallback = (string message) => 
            {
                Debug.Log($"Banner Error: {message}");
                OnAdFailedToLoad();
            }
        };

        Advertisement.Banner.Load(id, options);
    }

    public new void Show()
    {
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnAdClick,
            showCallback = OnAdShow,
            hideCallback = OnAdHide
        };
        Advertisement.Banner.Show(id, options);
    }
    public void Hide()
    {
        Advertisement.Banner.Hide();
    }
    public override void OnAdClick()
    {
    }
    public override void OnAdFailedToLoad()
    {
        Debug.Log("Failed to load banner");
    }
    public override void OnAdShow()
    {
    }
    public void OnAdHide()
    {
    }
    public override void OnAdWatched(UnityAdsShowCompletionState showCompletionState)
    {
    }
}