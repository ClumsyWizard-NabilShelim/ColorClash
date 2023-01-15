using System.Collections;
using TMPro;
using UnityEngine;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentHighScore;

    private void Start()
    {
        if (PlayerDataManager.PlayerData.HighScore == 0)
            currentHighScore.transform.parent.gameObject.SetActive(false);
        else
            currentHighScore.text = $"<color=#FF5D5D>HighScore</color>\n {PlayerDataManager.PlayerData.HighScore}";

        AdsManager.GetAd(AdType.Interstitial, (InterstitialAds ad) =>
        {
            ad.Setup(null, () =>
            {
                Application.Quit();
            });
        });

        AdsManager.GetAd(AdType.Banner, (BannerAds ad) =>
        {
            ad.Setup(() =>
            {
                ad.Show();
            }, 
            UnityEngine.Advertisements.BannerPosition.BOTTOM_CENTER);
        });
    }

    public void Play()
    {
        SceneManagement.Load("ArenaSelection");
    }
    public void Shop()
    {
        SceneManagement.Load("Shop");
    }
    public void Credits()
    {
        SceneManagement.Load("Credits");
    }
    public void Quit()
    {
        AdsManager.GetAd(AdType.Interstitial, (InterstitialAds ad) =>
        {
            ad.Show();
        });
    }
}