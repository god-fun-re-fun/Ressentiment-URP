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
        // 싱글톤 인스턴스를 초기화합니다.
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
        // Renderer 컴포넌트를 가져옵니다.
        sphere = GetComponent<Renderer>();

        _TopColor = sphere.material.GetColor("_TopColor");
        _BottomColor = sphere.material.GetColor("_BottomColor");
        _Rim_Color = sphere.material.GetColor("_Rim_Color");
    }

    bool IsGray(double r, double g, double b, double tolerance = 0.05)
    {
        return Math.Abs(r - g) <= tolerance && Math.Abs(g - b) <= tolerance && Math.Abs(r - b) <= tolerance;
    }

    void CloneAndActivateGray()
    {
        if(clonedBlue)
        {
            clonedBlue = false;
            Destroy(cloneBlue);
        }
        if(clonedGreen)
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


    public void UpdateWorld_TopColor(Color rere)
    {
        float new_r = _TopColor.r + (rere.r - _TopColor.r) / velocity;
        float new_g = _TopColor.g + (rere.g - _TopColor.g) / velocity;
        float new_b = _TopColor.b + (rere.b - _TopColor.b) / velocity;

        _TopColor = new Color(new_r, new_g, new_b);
        sphere.material.SetColor("_TopColor", _TopColor);
        
        if(IsGray(new_r,new_g,new_b))
        {
            if(clonedGray == false)
            {
                CloneAndActivateGray();
            }
            
            blueCity.SetActive(false);
            greenCity.SetActive(false);
            grayCity.SetActive(true);
            fogController.ToGray();
        }
        else if(new_g >= 0.75)
        {
            if(clonedGreen == false)
            {
                CloneAndActivateGreen();
            }
            
            grayCity.SetActive(false);
            blueCity.SetActive(false);
            greenCity.SetActive(true);
            fogController.ToGreen();
        }
        else if (new_b >= 0.75)
        {
            if(clonedBlue == false)
            {
                CloneAndActivateBlue();
            }
            
            grayCity.SetActive(false);
            greenCity.SetActive(false);
            blueCity.SetActive(true);
            fogController.ToBlue();
        }


        if (new_r + 0.2 > 1.0f)
        {
            new_r = 0.8f;
        }
        if(new_g + 0.2 > 1.0f)
        {
            new_g = 0.8f;
        }
        if(new_b + 0.2 > 1.0f)
        {
            new_b = 0.8f;
        }
        //worldLight.color = new Color(new_r + 0.2f, new_g + 0.2f, new_b + 0.2f);
        billboardLight.color = new Color(new_r + 0.2f, new_g + 0.2f, new_b + 0.2f);
    }

    public void UpdateWorld_BottomColor(Color rere)
    {
        float new_r = _BottomColor.r + (rere.r - _BottomColor.r) / velocity;
        float new_g = _BottomColor.g + (rere.g - _BottomColor.g) / velocity;
        float new_b = _BottomColor.b + (rere.b - _BottomColor.b) / velocity;

        _BottomColor = new Color(new_r, new_g, new_b);
        sphere.material.SetColor("_BottomColor", _BottomColor);
    }

    public void UpdateWorld_Rim_Color(Color rere)
    {
        float new_r = _Rim_Color.r + (rere.r - _Rim_Color.r) / velocity;
        float new_g = _Rim_Color.g + (rere.g - _Rim_Color.g) / velocity;
        float new_b = _Rim_Color.b + (rere.b - _Rim_Color.b) / velocity;

        _Rim_Color = new Color(new_r, new_g, new_b);
        sphere.material.SetColor("_Rim_Color", _Rim_Color);
    }
}
