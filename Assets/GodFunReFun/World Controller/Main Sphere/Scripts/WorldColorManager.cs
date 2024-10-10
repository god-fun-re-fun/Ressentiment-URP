using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldColorManager : MonoBehaviour
{
    private static WorldColorManager _instance;
    public static WorldColorManager Instance { get { return _instance; } }

    Renderer sphere;
    public Color _TopColor;
    public Color _BottomColor;
    public Color _Rim_Color;

    public float velocity;

    //public Light worldLight;
    public Light billboardLight;

    public GameObject grayCity;
    public GameObject greenCity;
    public GameObject blueCity;

    public GameObject grayPeople;
    public GameObject greenPeople;
    public GameObject bluePeople;
    private GameObject cloneGray;
    bool clonedGray = false;
    private GameObject cloneGreen;
    bool clonedGreen = false;
    private GameObject cloneBlue;
    bool clonedBlue = false;

    public FogController fogController;


    void Awake()
    {
        // 싱글톤 인스턴스를 초기화
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sphere = GetComponent<Renderer>();

        _TopColor = sphere.material.GetColor("_TopColor");
        _BottomColor = sphere.material.GetColor("_BottomColor");
        _Rim_Color = sphere.material.GetColor("_Rim_Color");
    }

    bool IsGray(double r, double g, double b, double tolerance = 0.2)
    {
        return Math.Abs(r - g) <= tolerance && Math.Abs(g - b) <= tolerance && Math.Abs(r - b) <= tolerance;
    }

    void CloneAndActivateGray()
    {
        if (clonedBlue)
        {
            clonedBlue = false;
            Destroy(cloneBlue);
        }
        if (clonedGreen)
        {
            clonedGreen = false;
            Destroy(cloneGreen);
        }

        cloneGray = Instantiate(grayPeople);
        cloneGray.SetActive(true);
        clonedGray = true;
    }

    void CloneAndActivateGreen()
    {
        if (clonedBlue)
        {
            clonedBlue = false;
            Destroy(cloneBlue);
        }
        if (clonedGray)
        {
            clonedGray = false;
            Destroy(cloneGray);
        }

        cloneGreen = Instantiate(greenPeople);
        cloneGreen.SetActive(true);
        clonedGreen = true;
    }

    void CloneAndActivateBlue()
    {
        if (clonedGray)
        {
            clonedGray = false;
            Destroy(cloneGray);
        }
        if (clonedGreen)
        {
            clonedGreen = false;
            Destroy(cloneGreen);
        }

        cloneBlue = Instantiate(bluePeople);
        cloneBlue.SetActive(true);
        clonedBlue = true;
    }

    public void UpdateWorld(Color rereColor)
    {
        // TopColor 갱신
        UpdateTopColor(rereColor);
        // 조건 확인 후 월드 전환 (안개, 오브젝트, npc)
        HandleCityActivation();
        // 조명 색상 업데이트
        UpdateLightColors();
    }
    
    public void UpdateTopColor(Color targetColor)
    {
        // TopColor 갱신
        float new_r = _TopColor.r + (targetColor.r - _TopColor.r) / velocity;
        float new_g = _TopColor.g + (targetColor.g - _TopColor.g) / velocity;
        float new_b = _TopColor.b + (targetColor.b - _TopColor.b) / velocity;

        _TopColor = new Color(new_r, new_g, new_b);
        sphere.material.SetColor("_TopColor", _TopColor);
    }

    public void UpdateBottomColor(Color rere)
    {
        float new_r = _BottomColor.r + (rere.r - _BottomColor.r) / velocity;
        float new_g = _BottomColor.g + (rere.g - _BottomColor.g) / velocity;
        float new_b = _BottomColor.b + (rere.b - _BottomColor.b) / velocity;

        _BottomColor = new Color(new_r, new_g, new_b);
        sphere.material.SetColor("_BottomColor", _BottomColor);
    }

    public void UpdateRimColor(Color rere)
    {
        float new_r = _Rim_Color.r + (rere.r - _Rim_Color.r) / velocity;
        float new_g = _Rim_Color.g + (rere.g - _Rim_Color.g) / velocity;
        float new_b = _Rim_Color.b + (rere.b - _Rim_Color.b) / velocity;

        _Rim_Color = new Color(new_r, new_g, new_b);
        sphere.material.SetColor("_Rim_Color", _Rim_Color);
    }

    // TopColor를 기준으로 옆면, rimColor가 정해지므로 TopColor기준으로 월드전환 컨트롤
    private void HandleCityActivation()
    {
        float new_r = _TopColor.r;
        float new_g = _TopColor.g;
        float new_b = _TopColor.b;

        if (IsGray(new_r, new_g, new_b))
        {
            if (clonedGray == false)
            {
                CloneAndActivateGray();
            }

            SetActiveCities(false, false, true);
            fogController.ToGray();
        }
        else if (new_g >= 0.75)
        {
            if (clonedGreen == false)
            {
                CloneAndActivateGreen();
            }

            SetActiveCities(false, true, false);
            fogController.ToGreen();
        }
        else if (new_b >= 0.75)
        {
            if (clonedBlue == false)
            {
                CloneAndActivateBlue();
            }

            SetActiveCities(true, false, false);
            fogController.ToBlue();
        }
    }

    private void SetActiveCities(bool blueActive, bool greenActive, bool grayActive)
    {
        blueCity.SetActive(blueActive);
        greenCity.SetActive(greenActive);
        grayCity.SetActive(grayActive);
    }

    private void UpdateLightColors()
    {
        float new_r = ClampColorValue(_TopColor.r);
        float new_g = ClampColorValue(_TopColor.g);
        float new_b = ClampColorValue(_TopColor.b);

        billboardLight.color = new Color(new_r + 0.2f, new_g + 0.2f, new_b + 0.2f);
    }

    private float ClampColorValue(float value)
    {
        return Mathf.Min(value + 0.2f, 0.8f);
    }
}