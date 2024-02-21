using System;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform defaultRespawnPoint;
    [SerializeField] private float verticalOfffset;

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
            other.transform.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y + verticalOfffset, respawnPoint.position.z);
            PlayerFell?.Invoke();
        }
    }
}