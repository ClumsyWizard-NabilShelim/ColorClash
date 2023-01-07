using ClumsyWizard.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMaster : Persistant<AudioMaster>
{
    [SerializeField] private AudioMixerGroup backgroundGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    private static bool isInitialized;
    private static Action onInitialize;

    protected override void Awake()
    {
        base.Awake();
        if(Instance != null)
        { 
            isInitialized = true;
            onInitialize?.Invoke();
        }
    }

    public static void GetBackgroundGroup(Action<AudioMixerGroup> callback)
    {
        if (!isInitialized)
            onInitialize += () => GetBackgroundGroup(callback);
        else
            callback(Instance.backgroundGroup);
    }

    public static void GetSFXGroup(Action<AudioMixerGroup> callback)
    {
        if (!isInitialized)
            onInitialize += () => GetSFXGroup(callback);
        else
            callback(Instance.sfxGroup);
    }

    protected override void CleanUp()
    {
    }
}
