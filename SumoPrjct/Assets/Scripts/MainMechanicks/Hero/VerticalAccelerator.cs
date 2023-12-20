using Sound_Player;
using UnityEngine;

public class VerticalAccelerator : MonoBehaviour
{
    [SerializeField] private SoundPlayer soundPlayer;
    [SerializeField] private float accelerationForce;
    private Rigidbody body;
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }
    public void SpeedUp()
    {
        soundPlayer.PlaySound(SoundName.Acceleration);
        body.AddForce(-transform.up * accelerationForce, ForceMode.Impulse); 
    }
}
