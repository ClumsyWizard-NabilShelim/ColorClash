using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Item", menuName = "Shop Item/Upgrade")]
public class UpgradeShopItemData : ShopItemData
{
    [field: SerializeField] public int Levels { get; private set; }
}
