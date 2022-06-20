using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class LoginManager : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    [SerializeField] private InputField Input_Id;
    [SerializeField] private InputField Input_Pw;
    [SerializeField] private Text errorLog;

    void Awake()
    {
        // 마스터 클라이언트와 일반 클라이언트들이 레벨을 동기화할지 결정
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void PressLoginBtn()
    {
        // photon 서버 연결하기
        if (!PhotonNetwork.IsConnected)
        {
            // 접속에 필요한 정보 설정
            PhotonNetwork.GameVersion = gameVersion;
            // 설정한 정보로 마스터 서버 접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
 
        string id = Input_Id.text;
        string pw = Input_Pw.text;

        if(string.IsNullOrEmpty(id))
        {
            errorLog.text = "아이디를 입력하세요";
        }
        else if (string.IsNullOrEmpty(pw))
        {
            errorLog.text = "비밀번호를 입력하세요";
        }
        else
        {
            PhotonNetwork.NickName = id;
            LoadLobby();
        }
    }

    public void LoadLobby()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN 연결 에러 원인 : {0}", cause);

        // 설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    
}
