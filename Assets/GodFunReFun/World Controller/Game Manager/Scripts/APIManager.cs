using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Net;

public class APIManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    private static APIManager _instance;
    public static APIManager Instance { get { return _instance; } }


    string apiUrl = "http://15.164.149.234:8080/interaction/result";

    public ResponseDto responseData { get; private set; }

    private void Awake()
    {
        // 싱글톤 인스턴스를 설정
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // 콜백 함수를 위한 Action 선언
    public System.Action onCompletedRequest;

    public void GetAPI()
    {
        StartCoroutine(GetRequest(apiUrl));
    }

    IEnumerator GetRequest(string url)
    {
        // 보안 프로토콜 설정 (HTTP 사용)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // 요청을 보냄
            yield return webRequest.SendWebRequest();

            // 오류 처리
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                // 성공적으로 받은 경우
                responseData = JsonUtility.FromJson<ResponseDto>(webRequest.downloadHandler.text);

                // 콜백 호출
                if (onCompletedRequest != null)
                {
                    onCompletedRequest.Invoke();
                }
            }
        }
    }
}