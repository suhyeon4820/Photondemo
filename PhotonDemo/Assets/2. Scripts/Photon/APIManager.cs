using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class APIManager : MonoBehaviour
{
    public static APIManager instance;
    private LobbyManager lobbyManager;

    private const string MainDns = "https://dev-world-server.readyplay.co.kr/";
    private const string Playworld = "api/playworld/";
    private const string TotalSerch = "?page=1&status=TOTAL";
    private const string WorldSpace = "api/spaces/";
    private const string liveInform = "/set-live";
    public string livestreamUrl;

    [HideInInspector] public List<string> playerList;
    [HideInInspector] public List<string> worldList;
    [HideInInspector] public List<string> playerIdList;
    [HideInInspector] public List<Texture> textureList;
    [HideInInspector] public List<string> textureArr;

    // 동적 버튼 생성 -----------------------------
    [SerializeField] private Transform thumbnailPnl;
    [SerializeField] private Button thumbnailBtn;


    void Awake()
    {
        instance = this;
        lobbyManager = GameObject.Find("Lobby").GetComponent<LobbyManager>();
        
    }
    public static APIManager Instance
    {
        get
        {
            if(null == instance)
            {
                instance = new APIManager();
            }
            return instance;
        }
    }

    public IEnumerator GetWorldInfrom()
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(MainDns + Playworld + TotalSerch))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError("eerrroorr");
            }

            DBManager.AllPlayworldData data = JsonUtility.FromJson<DBManager.AllPlayworldData>(uwr.downloadHandler.text);

            
            switch (data.res_code)
            {
                case 200:
                    List<string> playerList_ = new List<string>();
                    List<string> worldList_ = new List<string>();
                    List<string> thumbnailTexts = new List<string>();
                    for (int i = 0; i < data.data.Length; i++)
                    {
                        playerList_.Add(data.data[i].id);    // id 입력받기
                        worldList_.Add(data.data[i].title);  // 제목 입력
                        thumbnailTexts.Add(data.data[i].thumbnailUrl); // 썸네일 url
                    }
                    playerList = playerList_;
                    worldList = worldList_;
                    textureArr = thumbnailTexts;

                    MakeIdButton(data.data.Length); // world 수 만큼 버튼 동적 생성
                    break;
                default:
                    Debug.LogError("error");
                    break;
            }
        }
    }

    // world 버튼 동적 생성 + onclick method 추가
    void MakeIdButton(int num)
    {
        StartCoroutine(GetPlayerID(playerList[1])); // 플레이어 id 가져오는 코루틴

        for (int i = 0; i < num; i++)
        {
            // 버튼 생성
            var button = Instantiate(thumbnailBtn, thumbnailPnl.transform);
            // 버튼 이미지 파싱해서 가져오는 코루틴 실행
            StartCoroutine(GetTexture(i, button));

            // onclick 메서드 설정 - 누르면 Photon Room 생성하게 함
            int number = i;
            Button tempButton = button.GetComponent<Button>();
            tempButton.onClick.AddListener(() => PressIDButton(number)); // 버튼 동적 생성 및 이벤트 할당
        }

    }

    void PressIDButton(int num)
    {
        Debug.Log(num);
        //RoomChoiceTg(num);  // 씬 생성 번호
        //CreateRoomBtn();    // photon 방 생성

        // world room 생성해야 함
        lobbyManager.CreateWorldRoom(num);
    }

    IEnumerator GetPlayerID(string id)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(MainDns + Playworld + id))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError("eerrroorr");
            }

            DBManager.PlayerIdData data = JsonUtility.FromJson<DBManager.PlayerIdData>(uwr.downloadHandler.text);

            switch (data.res_code)
            {
                case 200:
                    List<string> playerIdList_ = new List<string>();

                    for (int i = 0; i < data.data.spaces.Length; i++)
                    {
                        playerIdList_.Add(data.data.spaces[i].id.ToString());    // id 입력받기
                    }
                    playerIdList = playerIdList_;
                    break;
                default:
                    Debug.LogError("error");
                    break;
            }
        }
        StartCoroutine(GetLiveUrl(playerIdList[2]));    // url 가져오는 코루틴
    }

    // url 가져오기
    IEnumerator GetLiveUrl(string id)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(MainDns + WorldSpace + id))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError("eerrroorr");
            }

            DBManager.SpaceData data = JsonUtility.FromJson<DBManager.SpaceData>(uwr.downloadHandler.text);

            switch (data.res_code)
            {
                case 200:
                    string url = data.data.spaceContent[0].url;
                    livestreamUrl = url;
                    //Debug.Log("url : "+ livestreamUrl);
                    break;
                default:
                    Debug.LogError("error");
                    break;
            }
        }
    }

    IEnumerator GetTexture(int num, Button btn)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(textureArr[num]))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.LogError("eerrroorr");
            }
            else
            {
                Texture2D myTexture = DownloadHandlerTexture.GetContent(uwr);

                //texture[num] = myTexture;

                Rect rect = new Rect(0, 0, myTexture.width, myTexture.height);
                // texture -> sprite 변환 및 버튼에 이미지 적용
                btn.transform.GetComponent<Image>().sprite = Sprite.Create(myTexture, rect, new Vector2(0.5f, 0.5f));
            }
        }
    }
}
