using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Arena Item", menuName = "Shop Item/Arena")]
public class ArenaShopItemData : ShopItemData
{
    [field: SerializeField] public int Index { get; protected set; }
    [field: SerializeField] public Sprite EnemyIcon { get; protected set; }
}
