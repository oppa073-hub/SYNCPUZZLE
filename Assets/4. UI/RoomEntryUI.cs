using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class RoomEntryUI : MonoBehaviour
{
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Button joinButton;
    string roomName;

    public void SetRoom(string roomname)
    {
        roomNameText.text = roomname;
        roomName = roomname;
    }
    public void OnClickJoinButton()
    {
        PhotonNetwork.JoinRoom(roomName);
    }


}
