using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixer : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixer;

    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider effectsVolume;
    private enum Mixers 
    {
        Master,
        Music,
        Effects
    }
    void Start()
    {
        SetSavedVolume();
    }

    public void ChangeMasterVolume(float volume)
    {
        mixer.audioMixer.SetFloat(Mixers.Master.ToString(), Mathf.Lerp(-80, 0, volume));
        PlayerPrefs.SetFloat(Mixers.Master.ToString(), volume);
    }
    public void ChangeMusicVolume(float volume)
    {
        mixer.audioMixer.SetFloat(Mixers.Music.ToString(), Mathf.Lerp(-80, 0, volume));
        PlayerPrefs.SetFloat(Mixers.Music.ToString(), volume);
    }
    public void ChangeEffectsVolume(float volume)
    {
        mixer.audioMixer.SetFloat(Mixers.Effects.ToString(), Mathf.Lerp(-80, 0, volume));
        PlayerPrefs.SetFloat(Mixers.Effects.ToString(), volume);
    }

    public void SetSavedVolume()
    {
        masterVolume.value = PlayerPrefs.GetFloat(Mixers.Master.ToString(), 1);
        musicVolume.value = PlayerPrefs.GetFloat(Mixers.Music.ToString(), 1);
        effectsVolume.value = PlayerPrefs.GetFloat(Mixers.Effects.ToString(), 1);
        mixer.audioMixer.SetFloat(Mixers.Master.ToString(), Mathf.Lerp(-80, 0, PlayerPrefs.GetFloat(Mixers.Master.ToString(), 1)));
        mixer.audioMixer.SetFloat(Mixers.Music.ToString(), Mathf.Lerp(-80, 0, PlayerPrefs.GetFloat(Mixers.Music.ToString(), 1)));
        mixer.audioMixer.SetFloat(Mixers.Effects.ToString(), Mathf.Lerp(-80, 0, PlayerPrefs.GetFloat(Mixers.Effects.ToString(), 1)));
    }
}
