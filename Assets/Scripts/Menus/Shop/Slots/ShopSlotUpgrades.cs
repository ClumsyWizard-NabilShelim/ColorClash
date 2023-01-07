using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopSlotUpgrades : ShopSlot
{
    private UpgradeShopItemData data;
    private int currentLevel;
    private Upgrade upgrade;

    public void Initialize(UpgradeShopItemData data)
    {
        this.data = data;
        currentLevel = PlayerDataManager.PlayerData.UnlockedUpgrades.ContainsKey(data.IDName) ? PlayerDataManager.PlayerData.UnlockedUpgrades[data.IDName] : 0;
        string buttonLabel = currentLevel == 0 ? $"Buy \n {data.Cost}" : currentLevel == data.Levels ? "Maxed" : $"Upgrade \n {data.Cost + ((currentLevel * 0.5f) * data.Cost)}";

        if (currentLevel == data.Levels)
            buyOrUpgrageButton.interactable = false;

        Initialize(data.Name, data.Description, data.Cost, data.Icon, data.Levels, currentLevel, buttonLabel);
        upgrade = (Upgrade)gameObject.AddComponent(Type.GetType("UP_" + data.IDName));
        upgrade.Initialize(data);
    }

    protected override void OnBuyOrUpgrade()
    {
        PlayerDataManager.PlayerData.AddUpgrade(data.IDName);
        upgrade.Activate();
        Initialize(data);
    }
}