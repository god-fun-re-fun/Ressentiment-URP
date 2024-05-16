using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    public Color grayFogColor;
    public float grayFogDensity;
    public Material graySky;

    public Color greenFogColor;
    public float greenFogDensity;

    public Color blueFogColor;
    public float blueFogDensity;
    public Material blueSky;
    void Start()
    {
        //RenderSettings.fogColor = grayFogColor; // 안개 색상 설정
        //RenderSettings.fogDensity = grayFogDensity; // 안개 밀도 설정
        ToGray();
    }

    public void ToGray()
    {
        RenderSettings.fogColor = grayFogColor;
        RenderSettings.fogDensity = grayFogDensity;
        RenderSettings.skybox = graySky;
    }
    public void ToGreen()
    {
        RenderSettings.fogColor = greenFogColor;
        RenderSettings.fogDensity = greenFogDensity;
        RenderSettings.skybox = graySky;
    }
    public void ToBlue()
    {
        RenderSettings.fogColor = blueFogColor;
        RenderSettings.fogDensity = blueFogDensity;
        RenderSettings.skybox = blueSky;
    }
}
