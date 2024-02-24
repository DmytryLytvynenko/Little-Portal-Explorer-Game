using Sound_Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHatch : MonoBehaviour
{
    [SerializeField] private bool isTrap;
    [SerializeField] private SoundEffectPlayer soundEffectPlayer;
    private Rigidbody body;
    private Quaternion defaultRotation;
    private Vector3 defaultposition;
    private bool ignoreCollision = false;


    private void OnEnable()
    {
        Respawn.PlayerFell += ResetHatch;
    }
    private void OnDisable()
    {
        Respawn.PlayerFell -= ResetHatch;
    }
    void Start()
    {
        defaultposition = transform.position;
        defaultRotation = transform.rotation;
        body = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (ignoreCollision)
            return;
        if (isTrap)
        {
            soundEffectPlayer.PlaySound(SoundName.Switch);
            body.isKinematic = false;
            ignoreCollision = true;
        }
    }
    private void ResetHatch()
    {
        transform.position = defaultposition;
        transform.rotation = defaultRotation;
        body.isKinematic = true;
        ignoreCollision = false;
    }
}
