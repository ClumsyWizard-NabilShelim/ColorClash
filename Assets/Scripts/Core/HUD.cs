using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : Singleton<HUD>
{
    [SerializeField] private GameObject statsUiContainer;
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI coin;

    protected override void Awake()
    {
        base.Awake();

        SceneManagement.OnNewSceneLoadedCore += () =>
        {
            ToggleUI();
        };
    }

    private void Start()
    {
        ToggleUI();
        UpdateUI();
    }

    private void ToggleUI()
    {
        if (SceneManagement.isGameScene)
        {
            statsUiContainer.SetActive(false);
        }
        else
        {
            statsUiContainer.SetActive(true);
        }
    }

    public static void UpdateUI()
    {
        Instance.coin.text = PlayerDataManager.PlayerData.Coin.ToString();
    }

    public void Settings()
    {
        SettingsMenu.Open();
    }
}
