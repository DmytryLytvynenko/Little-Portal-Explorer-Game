using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;
using Sirenix.OdinInspector;


namespace Sound_Player
{
[Serializable]

    public enum SoundName
    {
        Walk,
        Attack,
        Throw,
        Jump,
        Acceleration,
        Fall,
        TakeDamage,
        Die,
        Explosion,
        Collision,
        Landing,
        Hover,
        Click,
        Close,
        WrongInput,
        SpecialSound,
        GameSaved,
        Switch,
        Portal
    }
    public class SoundPlayer : MonoBehaviour
    {
        [SerializedDictionary("SoundName","Sound")]
        [SerializeField] private  SerializedDictionary<SoundName,AudioClip> sounds;
        [SerializeField] private  AudioSource audioSource;

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
}
