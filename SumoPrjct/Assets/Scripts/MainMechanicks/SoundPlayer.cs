using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class SoundPlayer : MonoBehaviour
{
    [SerializedDictionary("SoundName","Sound")]
    [SerializeField] private  SerializedDictionary<SoundName,AudioClip> sounds;
    [SerializeField] private  AudioSource audioSource;

    public enum SoundName
    {
        Walk,
        Attack,
        Throw,
        Jump,
        TakeDamage,
        Die,
        Explosion,
        Collision,
        Hover,
        Click,
        WrongInput
    }

    public void PlaySound(string _key)
    {
        SoundName key = (SoundName)Enum.Parse(typeof(SoundName), _key);
        if (sounds.TryGetValue(key, out AudioClip clip))
            audioSource.PlayOneShot(clip);
    }
    public void PlaySound(SoundName key)
    {
        if (sounds.TryGetValue(key, out AudioClip clip))
            audioSource.PlayOneShot(clip);
    }
}
