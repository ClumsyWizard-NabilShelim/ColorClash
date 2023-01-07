using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopSlotPowerUp : ShopSlot
{
    private PowerUpShopItemData data;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private TextMeshProUGUI equipLabel;
    private Action onEquipClicked;
    private int currentLevel;

    public void Initialize(PowerUpShopItemData data)
    {
        this.data = data;
        currentLevel = PlayerDataManager.PlayerData.UnlockedPowerUps.ContainsKey(data.IDName) ? PlayerDataManager.PlayerData.UnlockedPowerUps[data.IDName] : 0;
        string buttonLabel = currentLevel == 0 ? $"Buy \n {data.Cost}" : currentLevel == data.Levels ? "Maxed" : $"Upgrade \n {data.Cost + ((currentLevel * 0.5f) * data.Cost)}";

        if (currentLevel == data.Levels)
            buyOrUpgrageButton.interactable = false;

        int rank = PlayerDataManager.PlayerData.UnlockedPowerUps[data.IDName];

        string modifiedDescription = data.Description.Replace("<DURATION>", ((int)data.GetDuraction(rank)).ToString());
        modifiedDescription = modifiedDescription.Replace("<EFFECTVALUE>", ((int)data.GetEffectValue(rank)).ToString());
        modifiedDescription = modifiedDescription.Replace("<COOLDOWN>", ((int)data.GetCoolDownDuration(rank)).ToString());
        Initialize(data.Name, modifiedDescription, data.Cost, data.Icon, data.Levels, currentLevel, buttonLabel);


        if (currentLevel == 0)
        {
            equipButton.SetActive(false);
        }
        else
        {
            equipButton.SetActive(true);
            if (PlayerDataManager.PlayerData.EquippedPowerUps.Contains(data.IDName))
            {
                equipLabel.text = "Remove";
                onEquipClicked = () =>
                {
                    PlayerDataManager.PlayerData.RemovePowerUp(data.IDName);
                    Initialize(this.data);
                };
            }
            else
            {
                equipLabel.text = "Equip";
                onEquipClicked = () =>
                {
                    if (PlayerDataManager.PlayerData.EquippedPowerUps.Count == PlayerDataManager.PlayerStats.MaxPowerUpSlots)
                        return;

                    PlayerDataManager.PlayerData.EquipPowerUp(data.IDName);
                    Initialize(this.data);
                };
            }
        }
    }

    protected override void OnBuyOrUpgrade()
    {
        PlayerDataManager.PlayerData.AddPowerUp(data.IDName);
        Initialize(data);
    }

    public void OnEquip()
    {
        onEquipClicked?.Invoke();
    }
}
