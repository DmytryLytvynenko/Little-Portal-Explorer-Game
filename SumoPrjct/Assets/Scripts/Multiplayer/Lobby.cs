using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Lobby : MonoBehaviourPunCallbacks 
{
    [SerializeField] private string multiplayerLevelName;

    [SerializeField] private TMP_InputField createRoomName;
    [SerializeField] private TMP_InputField joinRoomName;

    public void CreateRoom()
    {
        if (createRoomName.text == string.Empty)
        {
            print("Enter room name");
            return;
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(createRoomName.text,roomOptions);
    }
    public void JoinRoom()
    {
        if (joinRoomName.text == string.Empty)
        {
            print("Enter room name");
            return;
        }
        PhotonNetwork.JoinRoom(joinRoomName.text);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(multiplayerLevelName);
    }
}
