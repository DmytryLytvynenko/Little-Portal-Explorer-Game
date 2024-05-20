using Photon.Pun;
using UnityEngine;

public class NetworkPlayerController : MonoBehaviour
{
    [SerializeField] private PhotonView PV;
    private void Awake()
    {
        if (!PV.IsMine)
        {
            //Destroy(GetComponentInChildren<Rigidbody>());
            Destroy(GetComponentInChildren<AudioListener>());
            Destroy(GetComponentInChildren<CameraController>().gameObject);
        }
    }
}