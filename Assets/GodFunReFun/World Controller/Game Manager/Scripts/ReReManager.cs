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
    public List<Transform> targets; // ī�޶� �̵��� Ÿ�� Transform ����Ʈ
    public float cameraSpeed = 0.1f;      // ī�޶� �̵� �ӵ�
    private int currentIndex = 0;   // ���� Ÿ�� �ε���
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


    private bool isCoroutineRunning = false; // �ڷ�ƾ ���� ���¸� �����ϴ� ����

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

        // 1. RGB���� HSL�� ��ȯ
        float h, s, l;
        RGBToHSL(baseColor, out h, out s, out l);

        // 2. ���� ���� ����
        // ����(Hue)�� �ణ �����Ͽ� ���� ���� ����
        float similarHue1 = (h + 0.02f) % 1.0f; // ���� ���� (+0.028)
        float similarHue2 = (h - 0.07f + 1.0f) % 1.0f; // ���� ���� (-0.028), ���� ������ ���� +1.0f �� % 1.0f

        // 3. HSL���� RGB�� ��ȯ
        Color similarColor1 = HSLToRGB(similarHue1, s, l);
        Color similarColor2 = HSLToRGB(similarHue2, s, l);

        // 4. ���� ������ Material�� ����
        waterRenderer.material.SetColor("_TopColor", similarColor1);
        WorldColorManager.Instance.UpdateWorld_TopColor(similarColor1);
        // ���� ���� 1 ����
        waterRenderer.material.SetColor("_BottomColor", baseColor);
        WorldColorManager.Instance.UpdateWorld_BottomColor(baseColor);
        // ���� ���� 2 ������ ���Ѵٸ� �ٸ� ��Ƽ���� Ȥ�� ������Ƽ ���
        waterRenderer.material.SetColor("_Rim_Color", similarColor2);
        WorldColorManager.Instance.UpdateWorld_Rim_Color(similarColor2);
        light.color = similarColor2;
    }


    IEnumerator MoveCameraToTargetAndBack()
    {
        isCoroutineRunning = true;

        // ��ǥ ��ġ�� �̵�
        while (Vector3.Distance(mainCamera.transform.position, createCam.position) > 0.1f)
        {
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, createCam.position, smoothSpeed * Time.deltaTime);
            mainCamera.transform.position = smoothedPosition;

            Quaternion targetRotation = Quaternion.Lerp(mainCamera.transform.rotation, createCam.rotation, smoothSpeed * Time.deltaTime);
            mainCamera.transform.rotation = targetRotation;

            yield return null;
        }

        yield return new WaitForSeconds(waitingTime); // ��ǥ ��ġ���� ��� ���

        // ���� ��ġ�� ���ƿ���
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
        // �ݹ����� API ��û �Ϸ� �� ����� �޼��带 ����
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
        // APIManager�� responseData�� ����Ͽ� ���� ���� ����
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
        // ����.
        if (Input.GetKeyDown(KeyCode.Space) && !isCoroutineRunning)
        {
            GetPeople();
        }

        if(Input.GetMouseButtonDown(0) && !isCoroutineRunning)
        {
            GetPeople();
        }
        

        // �ڷ�ƾ�� ���� ���� �ƴ� ���� MoveToNextTargetSmoothly() ȣ��
        if (!isCoroutineRunning)
        {
            MoveToNextTargetSmoothly();
        }

        //�������
        //MoveToNextTargetSmoothly();
    }

    void MoveToNextTargetSmoothly()
    {
        if (targets.Count == 0) return; // Ÿ���� ������ �Լ� ����

        // ���� Ÿ������ ī�޶� �ε巴�� �̵�
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

        // Lerp �Լ��� ����Ͽ� ���� ��ġ�� ��ǥ ��ġ ���̸� �ε巴�� �̵�
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetTransform.position, cameraSpeed * Time.deltaTime);

        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, targetTransform.rotation, cameraSpeed * Time.deltaTime);

        // ī�޶� ���� Ÿ�ٿ� ���� �����ߴ��� üũ (�Ÿ��� �ſ� �����������)
        if (Vector3.Distance(mainCamera.transform.position, targetTransform.position) < range)
        {
            // ���� Ÿ������ �ε��� ������Ʈ (��ȯ)
            currentIndex = (currentIndex + 1) % targets.Count;
        }
    }
}
