using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyLightChange : MonoBehaviour
{
    public float changeSpeed = 0.05f; // 색상 변화에 걸리는 시간 (초)

    public Light light;

    private bool isColorChanging = false;
    private bool isColorReturning = false;

    Color lightColor;

    Color targetLightColor;

    void Start()
    {
        lightColor = light.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            GameObject rere = other.gameObject;
            MeshRenderer meshRenderer = rere.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = rere.AddComponent<MeshRenderer>();
            }

            Transform waterTransform = rere.transform.Find("Sphere");

            Renderer waterRenderer = waterTransform.GetComponent<SkinnedMeshRenderer>();

            targetLightColor = waterRenderer.material.GetColor("_TopColor");

            isColorChanging = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            targetLightColor = lightColor;

            isColorReturning = true;
        }
    }


    private void Update()
    {
        if (isColorChanging)
        {
            // 보간하여 색상을 부드럽게 변경
            light.color = Color.Lerp(light.color, targetLightColor, changeSpeed * Time.deltaTime);

            // 목표 색에 근접하면 색 변화 종료
            if (Vector4.Distance(light.color, targetLightColor) < 0.01f)
            {
                light.color = targetLightColor; // 목표 색으로 설정
                isColorChanging = false; // 색 변화 종료
            }
        }
        if (isColorReturning)
        {
            // 보간하여 색상을 부드럽게 변경
            light.color = Color.Lerp(light.color, targetLightColor, changeSpeed * Time.deltaTime);

            // 목표 색에 근접하면 색 변화 종료
            if (Vector4.Distance(light.color, targetLightColor) < 0.01f)
            {
                light.color = targetLightColor; // 목표 색으로 설정
                isColorReturning = false; // 색 변화 종료
            }
        }
    }
}
