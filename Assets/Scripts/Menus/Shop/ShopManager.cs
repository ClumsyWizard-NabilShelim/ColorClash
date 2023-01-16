using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private GameObject[] tabContainers;
    [SerializeField] private GameObject upgradeSlotPrefab;
    [SerializeField] private GameObject powerUpSlotPrefab;
    [SerializeField] private Transform powerUpsContainer;
    [SerializeField] private Transform upgradesContainer;
    private int powerUpsCount;
    private int upgradesCount;

    private int slotIndex;

    private void Start()
    {
        for (int i = 0; i < tabContainers.Length; i++)
        {
            tabContainers[i].SetActive(false);
        }

        tabContainers[0].SetActive(true);
        SetupPowerUpShop();
        SetupUpgradesShop();

        AdsManager.GetAd(AdType.Banner, (BannerAds ad) =>
        {
            ad.Setup(() =>
            {
                ad.Show();
            },
            UnityEngine.Advertisements.BannerPosition.BOTTOM_CENTER);
        });
    }

    private void SetupPowerUpShop()
    {
        AssetLoader.LoadAssetsByTag("PowerUp", (List<PowerUpShopItemData> datas) =>
        {
            powerUpsCount = datas.Count;
            foreach (PowerUpShopItemData data in datas)
            {
                ShopSlotPowerUp shopSlotPowerUp = Instantiate(powerUpSlotPrefab, powerUpsContainer).GetComponent<ShopSlotPowerUp>();
                shopSlotPowerUp.Initialize(data);
            }
        });
    }

    private void SetupUpgradesShop()
    {
        AssetLoader.LoadAssetsByTag("Upgrade", (List<UpgradeShopItemData> datas) =>
        {
            upgradesCount = datas.Count;
            foreach (UpgradeShopItemData data in datas)
            {
                ShopSlotUpgrades shopSlotUpgrades = Instantiate(upgradeSlotPrefab, upgradesContainer).GetComponent<ShopSlotUpgrades>();
                shopSlotUpgrades.Initialize(data);
            }
        });
    }

    public void Next()
    {
        tabContainers[slotIndex].SetActive(false);

        if (slotIndex == tabContainers.Length - 1)
            slotIndex = 0;
        else
            slotIndex++;

        tabContainers[slotIndex].SetActive(true);
    }
    public void Previous()
    {
        tabContainers[slotIndex].SetActive(false);

        if (slotIndex == 0)
            slotIndex = tabContainers.Length - 1;
        else
            slotIndex--;
        tabContainers[slotIndex].SetActive(true);
    }

    public void Back()
    {
        SceneManagement.Load("MainMenu");
    }

    public static void CheckForAchievements()
    {
        if(PlayerDataManager.PlayerData.UnlockedUpgrades.Count == Instance.upgradesCount)
        {
            AchievementManager.UnlockAchievement(GPGSIds.achievement_godly);
        }
        if (PlayerDataManager.PlayerData.UnlockedPowerUps.Count == Instance.powerUpsCount)
        {
            AchievementManager.UnlockAchievement(GPGSIds.achievement_unstoppable);
        }
    }
}
