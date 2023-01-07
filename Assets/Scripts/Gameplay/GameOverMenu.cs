using ClumsyWizard.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : Singleton<GameOverMenu>
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject highScoreText;
    private Animator animator;

    [SerializeField] private Button doubleCoinButton;
    [SerializeField] private Button doubleScoreButton;
    private int coinEarned;
    private int scoreEarned;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        doubleCoinButton.interactable = false;
        doubleScoreButton.interactable = false;

        AdsManager.GetAd(AdType.Rewareded, (RewardedAds ad) =>
        {
            ad.Setup(() =>
            {
                Instance.doubleCoinButton.interactable = true;
                Instance.doubleScoreButton.interactable = true;
            });
        });
    }

    public static void Over(int score, int coin, bool isHighScore)
    {
        Instance.coinEarned = coin;
        Instance.scoreEarned = score;
        GameManager.IsGameOver = true;
        Instance.animator.SetBool("Open", true);
        Instance.scoreText.text = score.ToString();
        Instance.coinText.text = coin.ToString();
        Instance.highScoreText.SetActive(isHighScore);
    }

    public void MainMenu()
    {
        DisplayAd(() =>
        {
            SceneManagement.Load("MainMenu");
        });
    }
    public void Retry()
    {
        DisplayAd(SceneManagement.Reload);
    }
    public void DoubleCoin()
    {
        doubleCoinButton.interactable = false;
        AdsManager.GetAd(AdType.Rewareded, (RewardedAds ad) =>
        {
            ad.Show(
                () =>
                {
                    PlayerDataManager.PlayerData.AddCoin(coinEarned);
                    Instance.coinText.text = (coinEarned * 2).ToString();
                    PlayerDataManager.Save();
                },
                () =>
                {
                    doubleCoinButton.interactable = true;
                }
            );
        });
    }
    public void DoubleScore()
    {
        doubleScoreButton.interactable = false;
        AdsManager.GetAd(AdType.Rewareded, (RewardedAds ad) =>
        {
            ad.Show(
                () =>
                {
                    Instance.highScoreText.SetActive((scoreEarned * 2) > PlayerDataManager.PlayerData.HighScore);
                    Instance.scoreText.text = (scoreEarned * 2).ToString();
                    PlayerDataManager.PlayerData.CheckAndSetHighScore(Mathf.CeilToInt(scoreEarned * 2));
                    PlayerDataManager.Save();
                },
                () =>
                {
                    doubleScoreButton.interactable = true;
                }
            );
        });
    }

    private void DisplayAd(Action onButtonCallback)
    {
        if (PlayerPrefs.GetInt(AdsSaveTags.LevelChangeCoolDown.ToString()) == 0)
        {
            AdsManager.GetAd(AdType.Interstitial, (InterstitialAds ad) =>
            {
                ad.Show(() =>
                {
                    SaveLoadManager.SetInt(AdsSaveTags.LevelChangeCoolDown.ToString(), AdsManager.InterstitialADCooldown + 1);
                });
            });
        }
        else
        {
            SaveLoadManager.DecrementInt(AdsSaveTags.LevelChangeCoolDown.ToString(), 1);
            onButtonCallback?.Invoke();
        }
    }
}
