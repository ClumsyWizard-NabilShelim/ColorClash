using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShopSlot : MonoBehaviour
{
    protected AudioManager audioManager;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image icon;
    [SerializeField] private Transform levelIndicatorHolder;
    [SerializeField] private GameObject levelIndicatorBlock;
    [SerializeField] protected Button buyOrUpgrageButton;
    [SerializeField] private TextMeshProUGUI buttonLabel;
    private int cost;

    public void Initialize(string name, string description, int cost, Sprite iconImg, int maxLevel, int currentLevel, string buttonLabel)
    {
        audioManager = GetComponentInChildren<AudioManager>();
        this.cost = cost;
        nameText.text = name;
        descriptionText.text = description;
        icon.sprite = iconImg;

        if(levelIndicatorHolder.childCount > 0)
        {
            for (int i = 0; i < levelIndicatorHolder.childCount; i++)
            {
                Destroy(levelIndicatorHolder.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < maxLevel; i++)
        {
            Image image = Instantiate(levelIndicatorBlock, levelIndicatorHolder).GetComponent<Image>();
            if (i < currentLevel)
                image.color = new Color(0.14f, 0.71f, 1.0f);
            else
                image.color = new Color(0.07f, 0.42f, 0.6f);
        }

        this.buttonLabel.text = buttonLabel;
    }

    public void OnClick()
    {
        if (PlayerDataManager.PlayerData.Coin >= cost)
        {
            audioManager.Play("BuyClick");
            PlayerDataManager.PlayerData.UseCoin(cost);
            OnBuyOrUpgrade();
        }
        else
        {
            audioManager.Play("Denied");
        }
    }

    protected abstract void OnBuyOrUpgrade();
}
