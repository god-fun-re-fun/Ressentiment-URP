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
        RenderSettings.fogColor = grayFogColor; // 안개 색상 설정
        RenderSettings.fogDensity = grayFogDensity; // 안개 밀도 설정
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
