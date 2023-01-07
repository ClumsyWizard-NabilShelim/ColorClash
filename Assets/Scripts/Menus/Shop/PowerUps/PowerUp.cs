using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUp : MonoBehaviour
{
    public PowerUpShopItemData Data { get; private set; }
    public AudioClip AudioClip { get; private set; }
    protected int Rank { get; private set; }
    [SerializeField] protected PowerUpButton button;
    private float currentTime;
    private float coolDownCurrentTime;
    private bool isUsing;
    private bool coolDown;

    public void Initialize(PowerUpShopItemData data, PowerUpButton button)
    {
        Data = data;
        AudioClip = data.AudioClip;
        this.button = button;
        Rank = PlayerDataManager.PlayerData.UnlockedPowerUps[data.IDName];
    }

    public void Activate()
    {
        if (coolDownCurrentTime > 0)
            return;

        currentTime = Data.GetDuraction(Rank);
        coolDownCurrentTime = Data.GetCoolDownDuration(Rank);
        Use();
        isUsing = true;
    }

    private void Update()
    {
        if (coolDown)
        {
            if (coolDownCurrentTime > 0)
            {
                coolDownCurrentTime -= Time.deltaTime;
                button.UpdateCoverAmount(coolDownCurrentTime / Data.GetCoolDownDuration(Rank));

            }
            else
            {
                coolDownCurrentTime = 0;
                coolDown = false;
            }
        }

        if (isUsing)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                isUsing = false;
                coolDown = true;
                Deactivate();
            }
        }
    }

    protected abstract void Use();
    protected abstract void Deactivate();
}
