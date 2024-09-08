using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ReReManager : MonoBehaviour
{
    public static ReReManager instance;
    public Transform createPos;
    public GameObject prefabPeople;

    public Camera mainCamera;
    public List<Transform> targets; // 카메라가 이동할 타겟 Transform 리스트
    public float cameraSpeed = 0.1f;      // 카메라 이동 속도
    private int currentIndex = 0;   // 현재 타겟 인덱스
    public Transform createCam;
    public Transform originCam;
    public float smoothSpeed = 0.1f;
    public float waitingTime = 10.0f;

    public Animator worldSymbol;
    public Animator worldSymbolGreen;
    public Animator worldSymbolBlue;

    //public string[] tags = { "P1", "P2" };

    public AudioSource gearTrigger;
    int randomIndex;
    string selectedTag;


    private bool isCoroutineRunning = false; // 코루틴 실행 상태를 추적하는 변수

    void Awake()
    {
        instance = this;
    }

    public void RGBToHSL(Color rgbColor, out float h, out float s, out float l)
    {
        h = s = l = 0;
        float r = rgbColor.r;
        float g = rgbColor.g;
        float b = rgbColor.b;
        float max = Mathf.Max(r, Mathf.Max(g, b));
        float min = Mathf.Min(r, Mathf.Min(g, b));
        l = (max + min) / 2;

        if (max == min)
        {
            h = s = 0; // achromatic
        }
        else
        {
            float d = max - min;
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
            if (max == r)
            {
                h = (g - b) / d + (g < b ? 6 : 0);
            }
            else if (max == g)
            {
                h = (b - r) / d + 2;
            }
            else if (max == b)
            {
                h = (r - g) / d + 4;
            }
            h /= 6;
        }
    }
    public Color HSLToRGB(float h, float s, float l)
    {
        float r, g, b;

        if (s == 0)
        {
            r = g = b = l; // achromatic
        }
        else
        {
            float q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            float p = 2 * l - q;
            r = HueToRGB(p, q, h + 1f / 3);
            g = HueToRGB(p, q, h);
            b = HueToRGB(p, q, h - 1f / 3);
        }

        return new Color(r, g, b);
    }

    float HueToRGB(float p, float q, float t)
    {
        if (t < 0f) t += 1f;
        if (t > 1f) t -= 1f;
        if (t < 1f / 6f) return p + (q - p) * 6f * t;
        if (t < 1f / 2f) return q;
        if (t < 2f / 3f) return p + (q - p) * (2f / 3f - t) * 6f;
        return p;
    }



    void ApplySimilarColors(Color baseColor)
    {
        GameObject nextReRe = Instantiate(ReReManager.instance.prefabPeople, createPos);

        //nextReRe.tag = selectedTag;

        MeshRenderer meshRenderer = nextReRe.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = nextReRe.AddComponent<MeshRenderer>();
        }

        Transform waterTransform = nextReRe.transform.Find("Sphere");
        Transform lightTransform = nextReRe.transform.Find("Point Light");

        Renderer waterRenderer = waterTransform.GetComponent<SkinnedMeshRenderer>();
        Light light = lightTransform.GetComponent<Light>();

        // 1. RGB에서 HSL로 변환
        float h, s, l;
        RGBToHSL(baseColor, out h, out s, out l);

        // 2. 유사 색상 생성
        // 색조(Hue)를 약간 조정하여 유사 색상 생성
        float similarHue1 = (h + 0.02f) % 1.0f; // 색조 조정 (+0.028)
        float similarHue2 = (h - 0.07f + 1.0f) % 1.0f; // 색조 조정 (-0.028), 음수 방지를 위해 +1.0f 후 % 1.0f

        // 3. HSL에서 RGB로 변환
        Color similarColor1 = HSLToRGB(similarHue1, s, l);
        Color similarColor2 = HSLToRGB(similarHue2, s, l);

        // 4. 유사 색상을 Material에 적용
        waterRenderer.material.SetColor("_TopColor", similarColor1);
        WorldColorManager.Instance.UpdateWorld_TopColor(similarColor1);
        // 유사 색상 1 적용
        waterRenderer.material.SetColor("_BottomColor", baseColor);
        WorldColorManager.Instance.UpdateWorld_BottomColor(baseColor);
        // 유사 색상 2 적용을 원한다면 다른 머티리얼 혹은 프로퍼티 사용
        waterRenderer.material.SetColor("_Rim_Color", similarColor2);
        WorldColorManager.Instance.UpdateWorld_Rim_Color(similarColor2);
        light.color = similarColor2;
    }


    IEnumerator MoveCameraToTargetAndBack()
    {
        isCoroutineRunning = true;

        // 목표 위치로 이동
        while (Vector3.Distance(mainCamera.transform.position, createCam.position) > 0.1f)
        {
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, createCam.position, smoothSpeed * Time.deltaTime);
            mainCamera.transform.position = smoothedPosition;

            Quaternion targetRotation = Quaternion.Lerp(mainCamera.transform.rotation, createCam.rotation, smoothSpeed * Time.deltaTime);
            mainCamera.transform.rotation = targetRotation;

            yield return null;
        }

        yield return new WaitForSeconds(waitingTime); // 목표 위치에서 잠시 대기

        // 원래 위치로 돌아오기
        while (Vector3.Distance(mainCamera.transform.position, originCam.position) > 0.1f)
        {
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, originCam.position, smoothSpeed * Time.deltaTime);
            mainCamera.transform.position = smoothedPosition;

            Quaternion smoothedRotation = Quaternion.Lerp(mainCamera.transform.rotation, originCam.rotation, smoothSpeed * Time.deltaTime);
            mainCamera.transform.rotation = smoothedRotation;

            yield return null;
        }

        isCoroutineRunning = false;
    }

    void GetPeople()
    {
        //randomIndex = Random.Range(0, tags.Length);
        //selectedTag = tags[randomIndex];

        //GameObject nextReRe = Instantiate(ReReManager.instance.prefabPeople, createPos);
        // 콜백으로 API 요청 완료 후 실행될 메서드를 지정
        APIManager.Instance.onCompletedRequest = ApplyColorFromAPI;
        APIManager.Instance.GetAPI();
        gearTrigger.Play();
        StartCoroutine(MoveCameraToTargetAndBack());
        worldSymbol.SetTrigger("Create");
        worldSymbolBlue.SetTrigger("Create");
        worldSymbolGreen.SetTrigger("Create");
    }

    void ApplyColorFromAPI()
    {
        // APIManager의 responseData를 사용하여 색상 적용 로직
        if (APIManager.Instance.responseData != null && APIManager.Instance.responseData.data != null)
        {
            double R = APIManager.Instance.responseData.data.r;
            double G = APIManager.Instance.responseData.data.g;
            double B = APIManager.Instance.responseData.data.b;
            Color color = new Color((float)R, (float)G, (float)B);
            ApplySimilarColors(color);
        }
    }

    void Update()
    {
        // 기존.
        if (Input.GetKeyDown(KeyCode.Space) && !isCoroutineRunning)
        {
            GetPeople();
        }

        if(Input.GetMouseButtonDown(0) && !isCoroutineRunning)
        {
            GetPeople();
        }
        

        // 코루틴이 실행 중이 아닐 때만 MoveToNextTargetSmoothly() 호출
        if (!isCoroutineRunning)
        {
            MoveToNextTargetSmoothly();
        }

        //웹빌드용
        //MoveToNextTargetSmoothly();
    }

    void MoveToNextTargetSmoothly()
    {
        if (targets.Count == 0) return; // 타겟이 없으면 함수 종료

        // 현재 타겟으로 카메라를 부드럽게 이동
        Transform targetTransform = targets[currentIndex];
        float range = 1.0f;

        /*
        if (currentIndex == 2)
        {
            cameraSpeed = 1.0f;
            range = 0.3f;

        }
        else if (currentIndex == 3)
        {
            cameraSpeed = 1.0f;
            range = 0.005f;
        }
        else
        {
            cameraSpeed = 0.3f;
            range = 0.005f;
        }
        */

        // Lerp 함수를 사용하여 현재 위치와 목표 위치 사이를 부드럽게 이동
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetTransform.position, cameraSpeed * Time.deltaTime);

        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetTransform.rotation, cameraSpeed * Time.deltaTime);

        // 카메라가 현재 타겟에 거의 도달했는지 체크 (거리가 매우 가까워졌는지)
        if (Vector3.Distance(mainCamera.transform.position, targetTransform.position) < range)
        {
            // 다음 타겟으로 인덱스 업데이트 (순환)
            currentIndex = (currentIndex + 1) % targets.Count;
        }
    }
}
