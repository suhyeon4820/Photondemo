using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//using System.Diagnostics;

public class LoadingManager : MonoBehaviour
{

    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text progressText;

    private float loadingTime = 1.0f;
    string p_Text;
    
    void Awake()
    {
        InitProgress();
        SetProgress(loadingTime);

        
        //StartCoroutine(GetAPI());
    }

    IEnumerator GetAPI()
    {
        IEnumerator aa = APIManager.instance.GetWorldInfrom();
        // 월드 정보 가져오고 world room 버튼 동적 생성
        yield return APIManager.instance.StartCoroutine(aa);
    }

    void InitProgress()
    {
        if (p_Text == null)
            p_Text = "Loading {0:N0}%"; // Loading 0% (소수점 없이 표기 + 천 단위 ,)

        SetProgress(0, true);   // progressBar에값 넣어주기
    }

    void SetProgress(float val, bool isDirect = false)
    {
        if (isDirect)
        {
            progressSlider.value = val;
            progressText.text = string.Format(p_Text, val * 100);
        }
        else
           StartCoroutine(LerpProgress(val));
    }

    IEnumerator LerpProgress(float dest)
    {
        float prev = progressSlider.value;  // 시작값
        float timer = 0, present = 0;
        while (true)
        {
            timer += Time.deltaTime;
            present = Mathf.Lerp(prev, dest, timer);    // 시작값, 끝값, 간격
            progressSlider.value = present;
            progressText.text = string.Format(p_Text, present * 100);
            // 1초
            if (timer >= 1.0F)
                break;
            yield return null;
        }
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.3f));

        if (dest >= 1)
            LoadLobbyScene();
    }

    public void LoadLobbyScene()
    {
        PhotonNetwork.LoadLevel(2);
    }

}
