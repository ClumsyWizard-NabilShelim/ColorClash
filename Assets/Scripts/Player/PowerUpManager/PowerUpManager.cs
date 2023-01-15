using ClumsyWizard.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PowerUpManager : Singleton<PowerUpManager>
{
    [Header("UI")]
    [SerializeField] private Transform powerUpHolder;
    [SerializeField] private GameObject powerUpButtonPrefab;
    [SerializeField] private RectTransform freezeEffect;
    private PostProcessVolume volume;
    private Vignette vignette;

    [Header("Masked Vignette")]
    [SerializeField] private Texture2D mask;
    [SerializeField] private Color tintColor;

    private void Start()
    {
        volume = FindObjectOfType<PostProcessVolume>();

        volume.profile.TryGetSettings(out vignette);

        AssetLoader.RecursiveLoad(PlayerDataManager.PlayerData.EquippedPowerUps.ToArray(), (List<PowerUpShopItemData> powerUpDatas) =>
        {
            if(powerUpDatas.Count == 0)
            {
                powerUpHolder.gameObject.SetActive(false);
                return;
            }

            foreach (PowerUpShopItemData data in powerUpDatas)
            {
                if (powerUpHolder.childCount == PlayerDataManager.PlayerStats.MaxPowerUpSlots)
                    break;

                PowerUpButton button = Instantiate(powerUpButtonPrefab, powerUpHolder).GetComponent<PowerUpButton>();
                PowerUp powerUp = ((PowerUp)gameObject.AddComponent(System.Type.GetType("PU_" + data.IDName)));
                powerUp.Initialize(data, button);
                button.Setup(powerUp);
            }
            freezeEffect.sizeDelta = new Vector2((powerUpDatas.Count) * 150.0f + (20 * powerUpDatas.Count), freezeEffect.sizeDelta.y);
        });

        SceneManagement.OnNewSceneLoaded += () =>
        {
            CloseVignette();
        };
    }

    public static void ClassicVignette()
    {
        Instance.vignette.enabled.value = true;
        Instance.vignette.active = true;

        Instance.vignette.mode.value = VignetteMode.Classic;
        Instance.vignette.color.value = Color.black;
        Instance.InvokeRepeating("OpenVignette", 0.0f, Time.unscaledDeltaTime);
    }
    public static void MakedVignette()
    {
        Instance.vignette.enabled.value = true;
        Instance.vignette.active = true;

        Instance.vignette.mode.value = VignetteMode.Masked;
        Instance.vignette.color.value = Instance.tintColor;
        Instance.vignette.mask.value = Instance.mask;
        Instance.InvokeRepeating("OpenVignette", 0.0f, Time.unscaledDeltaTime);
    }

    private void OpenVignette()
    {
        if (vignette.mode.value == VignetteMode.Classic)
        {
            vignette.intensity.value += Time.unscaledDeltaTime * 5.0f;
            if (vignette.intensity.value >= 0.2f)
            {
                vignette.intensity.value = 0.2f;
                CancelInvoke("OpenVignette");
            }
        }
        else
        {
            vignette.opacity.value += Time.unscaledDeltaTime * 5.0f;
            if (vignette.opacity.value >= 1.0f)
            {
                vignette.opacity.value = 1.0f;
                CancelInvoke("OpenVignette");
            }
        }
    }

    public static void CloseVignette()
    {
        Instance.InvokeRepeating("FadeVignette", 0.0f, Time.deltaTime);
    }

    private void FadeVignette()
    {
        if (vignette.mode.value == VignetteMode.Classic)
        {
            vignette.intensity.value -= Time.deltaTime * 5.0f;
            if (vignette.intensity.value <= 0.05f)
            {
                vignette.intensity.value = 0.0f;
                Instance.vignette.active = false;
                CancelInvoke("FadeVignette");
            }
        }
        else
        {
            vignette.opacity.value -= Time.deltaTime * 5.0f;
            if (vignette.opacity.value <= 0.05f)
            {
                vignette.opacity.value = 0.0f;
                Instance.vignette.active = false;
                CancelInvoke("FadeVignette");
            }
        }
    }
}
