using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Power Up Item", menuName = "Shop Item/Power Up")]
public class PowerUpShopItemData : ShopItemData
{
    [SerializeField] private int levels;
    [SerializeField] private float duration;
    [SerializeField] private float coolDownDuration;
    [SerializeField] private float effectValue;
    [SerializeField] private float effectIncreasePerLevel;
    [field: SerializeField] public AudioClip AudioClip { get; private set; }

    public int Levels { get => levels; }

    public float GetDuraction(int rank)
    {
        if (rank == 0)
            return duration;

        return duration + ((0.2f * (rank - 1)) * duration);
    }
    public float GetCoolDownDuration(int rank)
    {
        if(rank == 0)
            return coolDownDuration;
        return coolDownDuration - ((0.2f * (rank - 1)) * coolDownDuration);
    }
    public float GetEffectValue(int rank)
    {
        if (rank == 0)
            return effectValue;
        return effectValue + (effectIncreasePerLevel * rank);
    }

    private void OnValidate()
    {
        Type = ShopItemType.PowerUps;
    }
}
