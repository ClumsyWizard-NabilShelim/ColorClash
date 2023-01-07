using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopItemType
{
    PowerUps,
    Upgrades,
    Cosmetics,
    Arena
}

public abstract class ShopItemData : ScriptableObject
{
    [field: SerializeField] public string Name { get; protected set; }
    public string IDName
    {
        get
        {
            string name = Name;
            CWString.NormalizeString(ref name);
            return name;
        }
    }
    [field: SerializeField, TextArea(5,5)] public string Description { get; protected set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public Sprite Icon { get; protected set; }
    [field: SerializeField] public ShopItemType Type { get; protected set; }
}
