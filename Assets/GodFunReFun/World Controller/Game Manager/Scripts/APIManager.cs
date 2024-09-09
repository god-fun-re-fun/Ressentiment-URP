using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Net;

public class APIManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    private static APIManager _instance;
    public static APIManager Instance { get { return _instance; } }


    string apiUrl = "http://15.164.149.234:8080/interaction/result";

    public ResponseDto responseData { get; private set; }

    private void Awake()
    {
        // �̱��� �ν��Ͻ��� ����
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // �ݹ� �Լ��� ���� Action ����
    public System.Action onCompletedRequest;

    public void GetAPI()
    {
        StartCoroutine(GetRequest(apiUrl));
    }

    IEnumerator GetRequest(string url)
    {
        // ���� �������� ���� (HTTP ���)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // ��û�� �����ϴ�.
            yield return webRequest.SendWebRequest();

            // ���� ó��
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                // ���������� ���� ���
                responseData = JsonUtility.FromJson<ResponseDto>(webRequest.downloadHandler.text);

                // �ݹ� ȣ��
                if (onCompletedRequest != null)
                {
                    onCompletedRequest.Invoke();
                }
            }
        }
    }
}
