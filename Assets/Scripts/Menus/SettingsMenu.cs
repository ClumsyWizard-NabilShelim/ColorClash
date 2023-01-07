using ClumsyWizard.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[System.Serializable]
public class SettingsData
{
    public float BackgroundVolume;
    public float SFXVolume;
    public bool CanShakeScreen;

    public SettingsData()
    {
        BackgroundVolume = 0.0f;
        SFXVolume = 0.0f;
        CanShakeScreen = true;
    }
}

public class SettingsMenu : Persistant<SettingsMenu>
{
    [SerializeField] private AudioMixer mixer;
    private SettingsData settings;
    private Animator animator;

    [SerializeField] private Slider backgroundMusicSlider;
    [SerializeField] private Slider sfxMusicSlider;
    [SerializeField] private Toggle canCameraShakeToggle;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SaveLoadManager.LoadData(SaveLoadKey.Settings, (SettingsData data) =>
        {
            if (data == null)
            {
                settings = new SettingsData();
            }
            else
            {
                settings = data;
            }

            backgroundMusicSlider.value = settings.BackgroundVolume;
            sfxMusicSlider.value = settings.SFXVolume;
            canCameraShakeToggle.isOn = settings.CanShakeScreen;

            mixer.SetFloat("BackgroundVolume", settings.BackgroundVolume);
            mixer.SetFloat("SFXVolume", settings.SFXVolume);
            CameraShake.CanShake(settings.CanShakeScreen);
        });
    }

    public static void Open()
    {
        Instance.animator.SetBool("Show", true);
    }
    public void Close()
    {
        Instance.animator.SetBool("Show", false);
        SaveLoadManager.SaveData(settings, SaveLoadKey.Settings);
    }

    public void SetBackgroundVolume(float value)
    {
        settings.BackgroundVolume = value;
        mixer.SetFloat("BackgroundVolume", value);
    }
    public void SetSFXVolume(float value)
    {
        settings.SFXVolume = value;
        mixer.SetFloat("SFXVolume", value);
    }
    public void ToggleScreenShake(bool value)
    {
        settings.CanShakeScreen = value;
        CameraShake.CanShake(value);
    }

    protected override void CleanUp()
    {}
}