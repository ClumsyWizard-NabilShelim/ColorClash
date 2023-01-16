using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaSlot : MonoBehaviour
{
    private AudioManager audioManager;
    private ArenaShopItemData data;
    private string arenaID;
    private int cost;
    private bool unlocked;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image icon;
    [SerializeField] private Image enemyIcon;
    [SerializeField] private TextMeshProUGUI buttonLabel;
    [SerializeField] private GameObject tint;

    public void Initialize(ArenaShopItemData data)
    {
        audioManager = GetComponentInChildren<AudioManager>();
        this.data = data;
        nameText.text = data.Name;
        descriptionText.text = data.Description;
        icon.sprite = data.Icon;
        enemyIcon.sprite = data.EnemyIcon;

        arenaID = data.IDName;
        unlocked = PlayerDataManager.PlayerData.UnlockedArenas.Contains(arenaID);
        cost = data.Cost;

        if (unlocked)
        {
            buttonLabel.text = "Start";
            tint.gameObject.SetActive(false);
        }
        else
        {
            buttonLabel.text = $"Buy ({cost})";
            tint.gameObject.SetActive(true);
        }
    }

    public void OnClick()
    {
        if (unlocked)
        {
            audioManager.Play("Click");
            SceneManagement.Load("Level_" + arenaID);
        }
        else
        {
            if (PlayerDataManager.PlayerData.Coin >= cost)
            {
                audioManager.Play("BuyClick");
                PlayerDataManager.PlayerData.UseCoin(cost);
                PlayerDataManager.PlayerData.AddArena(arenaID);

                if(arenaID == "Clouds")
                {
                    AchievementManager.UnlockAchievement(GPGSIds.achievement_peace);
                }
                else if (arenaID == "Arctic")
                {
                    AchievementManager.UnlockAchievement(GPGSIds.achievement_freezing);
                }
                else if (arenaID == "LavaLand")
                {
                    AchievementManager.UnlockAchievement(GPGSIds.achievement_warmth);
                }
                else if (arenaID == "Space")
                {
                    AchievementManager.UnlockAchievement(GPGSIds.achievement_void);
                }

                if(PlayerDataManager.PlayerData.UnlockedArenas.Count == 5)
                {
                    AchievementManager.UnlockAchievement(GPGSIds.achievement_traveller);
                }

                Initialize(data);
            }
            else
            {
                audioManager.Play("Denied");
            }
        }
    }
}
