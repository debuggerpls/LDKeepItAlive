using System;
using Unity.Audio;
using UnityEditor;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] sounds;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.pitch = sound.pitch;
        }
    }

    public void Play(string name)
    {
        var s = Array.Find(sounds, sound => sound.name == name);
        s?.source.Play();
    }
}

[Serializable]
public class Sound
{
    public string name;
    
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume;

    [Range(1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}
