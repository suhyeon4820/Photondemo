using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
//using Hashtable = ExitGames.Client.Photon.Hashtable;

using UnityEngine.Networking;
using System;
public class LobbyManager : MonoBehaviourPunCallbacks
{

    // 룸 목록을 저장하기 위한 딕셔터리 자료형
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    List<RoomInfo> Rooms = new List<RoomInfo>();

    // 동적 생성용
    [HideInInspector] public List<int> idList;
    [HideInInspector] public List<string> titleList;
    [HideInInspector] public List<string> resourcePathList;
    
    // 룸리스트 동적 버튼 생성 -----------------------------
    [SerializeField] private Transform roomLIstPnl;
    [SerializeField] private GameObject roomBtn;

    // UI -----------------------------------------
    [SerializeField] private Text playerName;
    [SerializeField] private Text playerCount;
    [SerializeField] private InputField inputRoomName;
    [SerializeField] private byte maxPlayers = 4;    // 0이면 무한
    
    private string sceneName = "scGreen";
    private string roomColorName = "Color.green";
    private string roomName = "";

    public Texture2D[] texture;

    void Awake()
    {
        
    }

    // 서버 접속 완료 콜백 - 마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("join the master");
            PhotonNetwork.JoinLobby();  // 로비 입장 함수
        }
        else
            Debug.Log("로비에 접속하지 못합");
    }

    void Update()
    {
        playerName.text = PhotonNetwork.NickName + "님 접속을 환영합니다.";
        playerCount.text = PhotonNetwork.CountOfPlayers.ToString() + "명 접속";
    }

    // 로비 접속 완료 콜백 - 마스터 서버의 로비에 입장 했을 때 호출
    public override void OnJoinedLobby()
    {
        Debug.Log("join the lobby");
        StartCoroutine(GetAPI());   // 로비에 입장하면 서버에 요청
    }

    IEnumerator GetAPI()
    {
        // 월드 정보 가져오고 world room 버튼 동적 생성
        yield return APIManager.instance.StartCoroutine(APIManager.instance.GetWorldInfrom());
    }

    // world room 생성 (API 연동) -----------------------------------------------
    public void CreateWorldRoom(int roomKind)
    {
        // 방 옵션 설정
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 0; // 무제한
        roomName = inputRoomName.text;
        // 룸 생성 함수 - roomName을 key값으로 room을 생성
        PhotonNetwork.CreateRoom(roomName, roomOptions, null);
    }

    // room 직접 생성 -----------------------------------------------
    // room 종류 토글키로 받아오는 함수
    public void RoomChoiceTg(int num)
    {
        string roomName = "";
        string colorName = "";
        // hexColor
        if (num == 0)
        {
            roomName = "scGreen";
            colorName = "#42f557";
        }
        else if (num == 1)
        {
            roomName = "scRed";
            colorName = "#f54242";
        }
        else if (num == 2)
        {
            roomName = "scBlue";
            colorName = "#4842f5";
        }
        else
        {
            roomName = "scRed";
            colorName = "#f54242";
        }
        sceneName = roomName;
        roomColorName = colorName;
    }

    public void CreateRoomBtn(int num)
    {
        Debug.Log("create");
        // 방 옵션 설정
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomName = inputRoomName.text;
        // 룸 프로퍼티는 문자열 키를 가진 해시테이블로 처리
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() {
        { "roomImage", roomColorName}};
        roomOptions.CustomRoomProperties = customProperties;
        // 이게 있어야 로비에 보여짐
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "roomImage" };

        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "방 번호 : ";
        }
        // 방 생성하기
        PhotonNetwork.CreateRoom(roomName, roomOptions, null);
    }

    // 룸 입장 완료 콜백 - 룸에 입장 했을 때 호출
    public override void OnJoinedRoom()
    {
        //Debug.Log("OnJoinedRoom");
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    Debug.Log("master");
        //    //PhotonNetwork.LoadLevel(sceneName);
        //    StartCoroutine(LoadRoom());
        //}
        //else
        //{
        //    Debug.Log("guest");
        //}

        //PhotonNetwork.LoadLevel(sceneName);
    }

    // 클라이언트가 룸을 생성하고 들어갔을 때 호출
    public override void OnCreatedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("master");
            PhotonNetwork.LoadLevel(sceneName);
        }
    }


    // room list ----------------------------------------------------------
    // 로비 내에 룸이 생성되거나 사라질때 자동 호출되는 콜백
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null; // 초기화

        foreach (var room in roomList)
        {
            // 룸이 삭제된 경우
            if (room.RemovedFromList == true)
            {
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);  // 룸 버튼 제거  
                roomDict.Remove(room.Name);
            }
            else // 룸 정보가 갱신(변경)된 경우
            {
                // 상시 룸은 리스트에 보여지지 않기
                if(room.MaxPlayers == 0)
                {
                    return;
                }
                else // 생성되는 방 리스트
                {
                    // 룸이 처음 생성된 경우
                    if (roomDict.ContainsKey(room.Name) == false)
                    {
                        GameObject _room = Instantiate(roomBtn, roomLIstPnl);   // 룸 버튼 생성
                        _room.GetComponent<RoomData>().RoomInfo = room; // 버튼에 룸 정보 입력
                        roomDict.Add(room.Name, _room);
                    }
                    else // 룸 정보를 갱신하는 경우
                    {
                        roomDict.TryGetValue(room.Name, out tempRoom);
                        tempRoom.GetComponent<RoomData>().RoomInfo = room;
                    }
                }
            }
        }
    }

    // 룸 생성 실패 콜백
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    // 룸 입장 실패 콜백
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log(message);
    }

    // 룸 랜덤입장 실패 콜백
    public override void OnJoinRandomFailed(short _returnCode, string _message)
    {
        
    }
    
}
