using UnityEngine;

public class VolumeSetter : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        audioMixer.SetSavedVolume();
    }
}
