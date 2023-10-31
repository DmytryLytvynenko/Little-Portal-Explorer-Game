using System;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform defaultRespawnPoint;

    public static Action PlayerFell;
    public Transform respawnPoint {private get;  set; }

    private void Start()
    {
        respawnPoint = defaultRespawnPoint;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            other.transform.position = respawnPoint.position;
            PlayerFell?.Invoke();
        }
    }
}
