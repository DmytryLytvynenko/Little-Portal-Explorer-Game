using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

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

    public void PlaySound(SoundName key)
    {
        if (sounds.TryGetValue(key, out AudioClip clip))
            audioSource.PlayOneShot(clip);
    }
}
