using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReReChange : MonoBehaviour
{
    public float changeSpeed = 5.0f; // 색상 변화에 걸리는 시간 (초)

    public Renderer sphere; //바뀌는 애

    private bool isColorChanging = false;
    private bool isColorReturning = false;

    Color rereColor;

    Color targetRereColor;

    void Start()
    {
        rereColor = sphere.material.GetColor("_Rim_Color");
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

            targetRereColor = waterRenderer.material.GetColor("_TopColor");

            isColorChanging = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            targetRereColor = rereColor;

            isColorReturning = true;
        }
    }


    private void Update()
    {
        if (isColorChanging)
        {
            // 보간하여 색상을 부드럽게 변경
            sphere.material.SetColor("_Rim_Color", Color.Lerp(sphere.material.GetColor("_Rim_Color"), targetRereColor, changeSpeed * Time.deltaTime));

            // 목표 색에 근접하면 색 변화 종료
            if (Vector4.Distance(sphere.material.GetColor("_Rim_Color"), targetRereColor) < 0.01f)
            {
                sphere.material.SetColor("_Rim_Color", targetRereColor); // 목표 색으로 설정
                isColorChanging = false; // 색 변화 종료
            }
        }
        if (isColorReturning)
        {
            // 보간하여 색상을 부드럽게 변경
            sphere.material.SetColor("_Rim_Color", Color.Lerp(sphere.material.GetColor("_Rim_Color"), targetRereColor, changeSpeed * Time.deltaTime));
            
            if (Vector4.Distance(sphere.material.GetColor("_Rim_Color"), targetRereColor) < 0.01f)
            {
                sphere.material.SetColor("_Rim_Color", targetRereColor); // 목표 색으로 설정
                isColorReturning = false; // 색 변화 종료                                           
            }
        }
    }
}
