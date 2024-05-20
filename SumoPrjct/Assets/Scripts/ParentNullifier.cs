using Photon.Pun;
using UnityEngine;

public class ParentNullifier : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform playerTransform;
    public override void OnJoinedRoom()
    {
        if (playerTransform.GetComponent<PhotonView>().IsMine)
            gameObject.SetActive(true);
    }
    void Start()
    {
        transform.parent = null;
    }
}