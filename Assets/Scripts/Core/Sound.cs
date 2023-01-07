using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [field: SerializeField] public string Name{ get; private set; }
    [field: SerializeField] public AudioClip Audio { get; private set; }
    [field: SerializeField] public float Volume { get; private set; }
    [field: SerializeField] public bool Loop{ get; private set; }
    [field: SerializeField] public bool PlayOnAwake{ get; private set; }
    public AudioSource AudioSource { get; set; }
}