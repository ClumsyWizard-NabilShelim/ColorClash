using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    protected UpgradeShopItemData Data { get; private set; }
    protected int Rank { get; private set; }

    public void Initialize(UpgradeShopItemData data)
    {
        Data = data;
        Rank = PlayerDataManager.PlayerData.UnlockedUpgrades[data.IDName];
    }

    public abstract void Activate();
}
