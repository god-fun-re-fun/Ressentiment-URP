using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReReManager : MonoBehaviour
{
    public static ReReManager instance;
    public Transform createPos;
    public GameObject prefabPeople;

    public Camera mainCamera;
    public Transform createCam;
    public Transform originCam;
    public float smoothSpeed = 0.1f;
    public float waitingTime = 10.0f;

    public Animator worldSymbol;
    public Animator headUpReRe01;
    public Animator headUpReRe02;

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
        float similarHue1 = (h + 0.07f) % 1.0f; // ���� ���� (+0.028)
        float similarHue2 = (h - 0.07f + 1.0f) % 1.0f; // ���� ���� (-0.028), ���� ������ ���� +1.0f �� % 1.0f

        // 3. HSL���� RGB�� ��ȯ
        Color similarColor1 = HSLToRGB(similarHue1, s, l);
        Color similarColor2 = HSLToRGB(similarHue2, s, l);

        // 4. ���� ������ Material�� ����
        waterRenderer.material.SetColor("_TopColor", baseColor);
        WorldColorManager.Instance.UpdateWorld_TopColor(baseColor);
        // ���� ���� 1 ����
        waterRenderer.material.SetColor("_BottomColor", similarColor1);
        WorldColorManager.Instance.UpdateWorld_BottomColor(similarColor1);
        // ���� ���� 2 ������ ���Ѵٸ� �ٸ� ��Ƽ���� Ȥ�� ������Ƽ ���
        waterRenderer.material.SetColor("_Rim_Color", similarColor2);
        WorldColorManager.Instance.UpdateWorld_Rim_Color(similarColor2);
        light.color = similarColor2;
    }


    IEnumerator MoveCameraToTargetAndBack()
    {
        // ��ǥ ��ġ�� �̵�
        while (Vector3.Distance(mainCamera.transform.position, createCam.position) > 0.01f)
        {
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, createCam.position, smoothSpeed * Time.deltaTime);
            mainCamera.transform.position = smoothedPosition;

            Quaternion targetRotation = Quaternion.Lerp(mainCamera.transform.rotation, createCam.rotation, smoothSpeed * Time.deltaTime);
            mainCamera.transform.rotation = targetRotation;

            yield return null;
        }

        yield return new WaitForSeconds(waitingTime); // ��ǥ ��ġ���� ��� ���

        // ���� ��ġ�� ���ƿ���
        while (Vector3.Distance(mainCamera.transform.position, originCam.position) > 0.01f)
        {
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, originCam.position, smoothSpeed * Time.deltaTime);
            mainCamera.transform.position = smoothedPosition;

            Quaternion smoothedRotation = Quaternion.Lerp(mainCamera.transform.rotation, originCam.rotation, smoothSpeed * Time.deltaTime);
            mainCamera.transform.rotation = smoothedRotation;

            yield return null;
        }
    }

    void GetPeople()
    {
        // �ݹ����� API ��û �Ϸ� �� ����� �޼��带 ����
        APIManager.Instance.onCompletedRequest = ApplyColorFromAPI;
        APIManager.Instance.GetAPI();
        StartCoroutine(MoveCameraToTargetAndBack());
        worldSymbol.SetTrigger("Create");
        headUpReRe01.SetTrigger("Create");
        headUpReRe02.SetTrigger("Create");
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetPeople();
        }
    }
}
