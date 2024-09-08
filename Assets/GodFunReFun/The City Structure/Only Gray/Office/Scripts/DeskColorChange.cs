using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeskColorChange : MonoBehaviour
{
    public float changeSpeed = 5.0f; // 색상 변화에 걸리는 시간 (초)

    private Material screenMaterial; // 오브젝트의 Material

    private bool isColorChanging = false;
    private bool isColorReturning = false;

    Color screenColor;

    Color targetScreenColor;

    void Start()
    {
        screenMaterial = GetComponent<Renderer>().material;

        screenColor = screenMaterial.GetColor("_EmissionColor");
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

            targetScreenColor = waterRenderer.material.GetColor("_TopColor");

            isColorChanging = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            targetScreenColor = screenColor;

            isColorReturning = true;
        }
    }


    private void Update()
    {
        if (isColorChanging)
        {
            // 보간하여 색상을 부드럽게 변경
            screenMaterial.SetColor("_EmissionColor", Color.Lerp(screenMaterial.GetColor("_EmissionColor"), targetScreenColor, changeSpeed * Time.deltaTime));

            // 목표 색에 근접하면 색 변화 종료
            if (Vector4.Distance(screenMaterial.color, targetScreenColor) < 0.01f)
            {
                screenMaterial.color = targetScreenColor; // 목표 색으로 설정
                isColorChanging = false; // 색 변화 종료
            }
        }
        if (isColorReturning)
        {
            // 보간하여 색상을 부드럽게 변경
            screenMaterial.SetColor("_EmissionColor", Color.Lerp(screenMaterial.GetColor("_EmissionColor"), targetScreenColor, (changeSpeed/4) * Time.deltaTime));

            // 목표 색에 근접하면 색 변화 종료
            if (Vector4.Distance(screenMaterial.color, targetScreenColor) < 0.01f)
            {
                screenMaterial.color = targetScreenColor; // 목표 색으로 설정
                isColorReturning = false; // 색 변화 종료
            }
        }
    }
}
