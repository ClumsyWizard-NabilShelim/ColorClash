using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpButton : MonoBehaviour
{
    private Action onClick;
    [SerializeField] private Image iconImg;
    [SerializeField] private Image cover;

    public void Setup(PowerUp powerUp)
    {
        if(powerUp.Data.AudioClip != null)
            GetComponent<AudioManager>().UpdateSoundClip(0, powerUp.Data.AudioClip);

        iconImg.sprite = powerUp.Data.Icon;
        onClick = powerUp.Activate;
        cover.gameObject.SetActive(false);
    }
    public void UpdateCoverAmount(float amount)
    {
        cover.fillAmount = amount;

        if (cover.fillAmount <= 0)
            cover.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if (PlayerManager.HasStatusEffet(StatusEffect.PowerUpFreeze))
            return;

        cover.gameObject.SetActive(true);
        onClick?.Invoke();
    }
}
