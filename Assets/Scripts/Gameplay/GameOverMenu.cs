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
        }, null);

        AdsManager.GetAd(AdType.Interstitial, (InterstitialAds ad) =>
        {
            ad.Setup(null, () =>
            {
                SaveLoadManager.SetInt(AdsSaveTags.LevelChangeCoolDown.ToString(), AdsManager.InterstitialADCooldown + 1);
            });
        }, null);
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

        Instance.AchievementCheck(coin);
        GPGSManager.PostToLeaderBoard(score);
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
        doubleScoreButton.interactable = false;
        AdsManager.GetAd(AdType.Rewareded, (RewardedAds ad) =>
        {
            ad.Show(
                () =>
                {
                    PlayerDataManager.PlayerData.AddCoin(coinEarned);
                    Instance.coinText.text = (coinEarned * 2).ToString();
                    Instance.AchievementCheck(coinEarned * 2);
                    PlayerDataManager.Save();
                },
                () =>
                {
                    doubleCoinButton.interactable = true;
                    doubleScoreButton.interactable = true;
                }
            );
        }, null);
    }
    public void DoubleScore()
    {
        doubleCoinButton.interactable = false;
        doubleScoreButton.interactable = false;
        AdsManager.GetAd(AdType.Rewareded, (RewardedAds ad) =>
        {
            ad.Show(
                () =>
                {
                    Instance.highScoreText.SetActive((scoreEarned * 2) > PlayerDataManager.PlayerData.HighScore);
                    Instance.scoreText.text = (scoreEarned * 2).ToString();
                    GPGSManager.PostToLeaderBoard(scoreEarned * 2);
                    PlayerDataManager.PlayerData.CheckAndSetHighScore(Mathf.CeilToInt(scoreEarned * 2));
                    PlayerDataManager.Save();
                },
                () =>
                {
                    doubleCoinButton.interactable = true;
                    doubleScoreButton.interactable = true;
                }
            );
        }, null);
    }

    private void DisplayAd(Action onButtonCallback)
    {
        if (PlayerPrefs.GetInt(AdsSaveTags.LevelChangeCoolDown.ToString()) == 0)
        {
            AdsManager.GetAd(AdType.Interstitial, (InterstitialAds ad) =>
            {
                ad.Show();
            }, onButtonCallback);
        }
        else
        {
            SaveLoadManager.DecrementInt(AdsSaveTags.LevelChangeCoolDown.ToString(), 1);
            onButtonCallback?.Invoke();
        }
    }

    private void AchievementCheck(int coin)
    {
        if (coin >= 500)
        {
            AchievementManager.UnlockAchievement(GPGSIds.achievement_money_craze);
        }
        if (coin >= 2000)
        {
            AchievementManager.UnlockAchievement(GPGSIds.achievement_money_monster);
        }
        if (coin >= 4000)
        {
            AchievementManager.UnlockAchievement(GPGSIds.achievement_money_maniac);
        }

        if (PlayerDataManager.PlayerAchievements.NetCoin >= 50000)
        {
            AchievementManager.UnlockAchievement(GPGSIds.achievement_loaded);
        }

        PlayerDataManager.PlayerAchievements.AddEnemiesKilled(ObjectSlicer.EnemiesKilled);

        if (PlayerDataManager.PlayerAchievements.EnemiesKilled >= 50)
        {
            AchievementManager.UnlockAchievement(GPGSIds.achievement_rookie);
        }
        if (PlayerDataManager.PlayerAchievements.EnemiesKilled >= 100)
        {
            AchievementManager.UnlockAchievement(GPGSIds.achievement_pro);
        }
        if (PlayerDataManager.PlayerAchievements.EnemiesKilled >= 200)
        {
            AchievementManager.UnlockAchievement(GPGSIds.achievement_ninja);
        }
        if (PlayerDataManager.PlayerAchievements.EnemiesKilled >= 300)
        {
            AchievementManager.UnlockAchievement(GPGSIds.achievement_maniac);
        }
    }
}
