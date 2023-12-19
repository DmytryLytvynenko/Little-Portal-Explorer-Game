using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializedDictionary("SoundName","Sound")]
    [SerializeField] private  SerializedDictionary<SoundName,AudioClip> sounds;
    [SerializeField] private  AudioSource audioSource;

    public enum SoundName
    {
        Walk,
        Attack,
        Jump,
        TakeDamage,
        Die,
        Explode,
        Collision
    }

    public void PlaySound(SoundName key)
    {
        if (sounds.TryGetValue(key, out AudioClip clip))
            audioSource.PlayOneShot(clip);
    }
}
