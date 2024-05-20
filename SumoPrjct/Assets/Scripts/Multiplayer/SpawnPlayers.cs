using UnityEngine;
using Photon.Pun;
using System.IO;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float xMin = -45;
    [SerializeField] private float xMax = 45;
    [SerializeField] private float yMin = -45;
    [SerializeField] private float yMax = 45;
    [SerializeField] private float spawnHeight = 1;
    void Start()
    {
        Vector3 playerPosition = new Vector3(Random.Range(xMin,xMax), spawnHeight,  Random.Range(yMin, yMax));
        string path = Path.Combine("PhotonPrefabs", "Player");
        PhotonNetwork.Instantiate(path, playerPosition,Quaternion.identity);
    }
}