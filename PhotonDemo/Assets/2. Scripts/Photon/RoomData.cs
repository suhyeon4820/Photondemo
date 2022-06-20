using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class RoomData : MonoBehaviour
{
    public Text[] texts;
    public Image roomImage;
    private RoomInfo _roomInfo; // photon 객체

    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            // 텍스트 정보 입력
            texts[0].text = _roomInfo.Name;
            texts[1].text = _roomInfo.PlayerCount.ToString() + " / " + _roomInfo.MaxPlayers.ToString();
            
            // 이미지 정보 입력
            ExitGames.Client.Photon.Hashtable cp = _roomInfo.CustomProperties;
            Color color;
            string colorName = cp["roomImage"].ToString();  // 룸 색상 정보 받아오기
            ColorUtility.TryParseHtmlString(colorName, out color);  // 문자열 색상으로 변경
            roomImage.color = color;    // 색상 입히기

            // 방으로 접속하는 이벤트 함수 연결
            GetComponent<Button>().onClick.AddListener(() => OnEnterRoom(_roomInfo.Name));
        }
    }

    private void Awake()
    {
        texts = gameObject.GetComponentsInChildren<Text>();
        roomImage = transform.GetChild(0).GetComponent<Image>();
    }

    void OnEnterRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}
