using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class sounds
{

    public AudioClip clip;
    public string name;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 6f)]
    public float pitch;

    public bool loop;
    [HideInInspector]
    public AudioSource source;

    public bool mute;
}


public class SoundManger : MonoBehaviour
{

    public sounds[] sounds;


    public void Awake()
    {
        foreach (sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.mute = s.mute;
        }
        Play("Music2");
    }
    public void Play(string name)
    {
        sounds s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void StopMusic(string name)
    {
        sounds s = Array.Find(sounds, sound => sound.name == name);
        s.source.mute = true;
    }
    public void AdjustVolumeGlobal(float volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].volume = volume;
            UpdateVolume();
        }

    }
    public void AdjustVolumeWithName(float volume, string name)
    {
        sounds s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = volume;

    }

    public void PlayRandomSound(string name, int count)
    {
        name += UnityEngine.Random.Range(1, count);
        sounds s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }


    public void StopMusic()
    {
       foreach(sounds s in sounds)
        {
            if (s.name == "Music2") s.mute = true;
        }
        UpdateVolume();
    }


    void UpdateVolume()
    {
        foreach (sounds s in sounds)
        {
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.mute = s.mute;
        }
    }

}
