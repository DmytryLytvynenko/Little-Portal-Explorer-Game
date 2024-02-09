using Sound_Player;
using UnityEngine;

public class VerticalAccelerator : MonoBehaviour
{
    [SerializeField] private SoundEffectPlayer soundEffectPlayer;
    [SerializeField] private float accelerationForce;
    private Rigidbody body;
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }
    public void SpeedUp()
    {
        soundEffectPlayer.PlaySound(SoundName.Acceleration);
        body.AddForce(-transform.up * accelerationForce, ForceMode.Impulse); 
    }
}
