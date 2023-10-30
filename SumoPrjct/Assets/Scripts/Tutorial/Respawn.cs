using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform defaultRespawnPoint;
    public Transform respawnPoint {private get;  set; }

    private void Start()
    {
        respawnPoint = defaultRespawnPoint;
    }
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = respawnPoint.position;
    }
}
