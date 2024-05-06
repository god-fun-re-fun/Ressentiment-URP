using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    public Color grayFogColor;
    public float grayFogDensity;

    public Color greenFogColor;
    public float greenFogDensity;

    void Start()
    {
        RenderSettings.fogColor = grayFogColor; // �Ȱ� ���� ����
        RenderSettings.fogDensity = grayFogDensity; // �Ȱ� �е� ����
    }

    public void ToGray()
    {
        RenderSettings.fogColor = grayFogColor;
        RenderSettings.fogDensity = grayFogDensity;
    }
    public void ToGreen()
    {
        RenderSettings.fogColor = greenFogColor;
        RenderSettings.fogDensity = greenFogDensity;
    }
}
