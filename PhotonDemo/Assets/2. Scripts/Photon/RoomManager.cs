using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;
using RenderHeads.Media.AVProVideo;
using RenderHeads.Media.AVProVideo.Demos.UI;


public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text welcomeText;
    
    // avprovideo ----------------------------
    [SerializeField] private MediaPlayer mediaPlayer;
    public string url = "";

    string colorName = "";
    private void Awake()
    {
        url = APIManager.instance.livestreamUrl;
        
        if (PhotonNetwork.IsConnected)
        {
            PlayMovie();
        }
    }

    // 룸 정보 갱신
    public void ChangeBtnColor(int num)
    {
        string roomColor = "";
        if (num == 0)
        {
            roomColor = "#f54242";
        }
        else if (num == 1)
        {
            roomColor = "#42f557";
        }
        else if (num == 2)
        {
            roomColor = "#4842f5";
        }
        colorName = roomColor;

        RoomInfoUpdate();
    }
    void RoomInfoUpdate()
    {
        if (PhotonNetwork.InRoom)
        {
            Room room = PhotonNetwork.CurrentRoom;
            ExitGames.Client.Photon.Hashtable cp = room.CustomProperties;
            cp["roomImage"] = colorName;
            room.SetCustomProperties(cp);
        }
    }

    void PlayMovie()
    {
        mediaPlayer.OpenMedia(new MediaPath(url, MediaPathType.AbsolutePathOrURL), autoPlay: true);
    }

    private void Start()
    {
        //Debug.Log(PhotonNetwork.CountOfPlayersInRooms + "명");
    }

    private void Update()
    {
        
    }

    // 방에서 나가기
    public void PressLeaveRoomBtn()
    {
        Debug.Log("1");
        PhotonNetwork.LeaveRoom();
    }

    
    public override void OnLeftRoom()
    {
        Debug.Log("2");
        PhotonNetwork.LoadLevel(1);
    }


    // 플레이어가 room으로 들어올 때
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
        welcomeText.text = "환영합니다.";
    }

    // 플레이어가 room에서 나갈 때
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


        }
    }

}
