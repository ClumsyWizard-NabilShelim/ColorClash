using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    [SerializeField] private bool isBackgroundMusic;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.AudioSource = gameObject.AddComponent<AudioSource>();
            sound.AudioSource.clip = sound.Audio;
            sound.AudioSource.volume = sound.Volume;
            sound.AudioSource.loop = sound.Loop;
            sound.AudioSource.playOnAwake = sound.PlayOnAwake;

            if(isBackgroundMusic)
            {
                AudioMaster.GetBackgroundGroup((AudioMixerGroup group) =>
                {
                    sound.AudioSource.outputAudioMixerGroup = group;
                });
            } 
            else
            {
                AudioMaster.GetSFXGroup((AudioMixerGroup group) =>
                {
                    sound.AudioSource.outputAudioMixerGroup = group;
                });
            }

            if (sound.PlayOnAwake)
                sound.AudioSource.Play();

            audioSources.Add(sound.Name, sound.AudioSource);
        }
    }

    public void Play(string name)
    {
        if (audioSources[name].isPlaying)
            return;

        if (!audioSources.ContainsKey(name))
        {
            Debug.Log("No audio with id: " + name);
            return;
        }

        audioSources[name].Play();
    }

    public void Stop(string name)
    {
        if (!audioSources[name].isPlaying)
            return;

        if (!audioSources.ContainsKey(name))
        {
            Debug.Log("No audio with id: " + name);
            return;
        }

        audioSources[name].Stop();
    }

    public void UpdateSoundClip(int index, AudioClip clip)
    {
        sounds[index].AudioSource.clip = clip;
    }
}
