using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorInteraction : MonoBehaviour
{
    public float changeDuration = 2.0f; // 색상 변화에 걸리는 시간 (초)

    private Material objMaterial; // 오브젝트의 Material

    public float velocity;

    public Light objLight;

    void Start()
    {
        objMaterial = GetComponent<Renderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameObject rere = other.gameObject;
            MeshRenderer meshRenderer = rere.GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                meshRenderer = rere.AddComponent<MeshRenderer>();
            }

            Transform waterTransform = rere.transform.Find("Sphere");

            Renderer waterRenderer = waterTransform.GetComponent<SkinnedMeshRenderer>();

            Color rereColor = waterRenderer.material.GetColor("_Rim_Color");
            Color objectColor = objMaterial.GetColor("_EmissionColor");
            float new_r = objectColor.r + (rereColor.r - objectColor.r) / velocity;
            float new_g = objectColor.g + (rereColor.g - objectColor.g) / velocity;
            float new_b = objectColor.b + (rereColor.b - objectColor.b) / velocity;

            Color lightColor = objLight.color;
            float new_lr = lightColor.r + (rereColor.r - lightColor.r) / velocity;
            float new_lg = lightColor.g + (rereColor.g - lightColor.g) / velocity;
            float new_lb = lightColor.b + (rereColor.b - lightColor.b) / velocity;

            rere.GetComponent<Animator>().SetBool("isEffecting", true);
            StartCoroutine(ChangeColor(objectColor, new Color(new_r, new_g, new_b), changeDuration, rere));
            StartCoroutine(ChangeLight(lightColor, new Color(new_lr + 0.2f, new_lg + 0.15f, new_lb + 0.1f), changeDuration));
        }
    }

    IEnumerator ChangeColor(Color objColor ,Color newColor, float duration, GameObject rere)
    {
        // 시작 색상을 Material의 현재 색상으로 설정합니다.
        Color startColor = objColor;
        // 변화를 시작하기 전의 시간을 기록합니다.
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            // 변화 진행 정도를 계산합니다. (0에서 1 사이의 값)
            float t = (Time.time - startTime) / duration;
            // 현재 색상을 계산합니다.
            Color currentColor = Color.Lerp(startColor, newColor, t);
            // Material의 색상을 업데이트합니다.
            objMaterial.SetColor("_EmissionColor", currentColor);
            // 다음 프레임까지 대기합니다.
            yield return null;
        }

        // 최종 목표 색상을 확실히 적용합니다.
        objMaterial.SetColor("_EmissionColor", newColor);
        rere.GetComponent<Animator>().SetBool("isEffecting", false);
    }

    IEnumerator ChangeLight(Color lighttColor, Color newColor, float duration)
    {
        // 시작 색상을 Material의 현재 색상으로 설정합니다.
        Color startColor = lighttColor;
        // 변화를 시작하기 전의 시간을 기록합니다.
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            // 변화 진행 정도를 계산합니다. (0에서 1 사이의 값)
            float t = (Time.time - startTime) / duration;
            // 현재 색상을 계산합니다.
            Color currentColor = Color.Lerp(startColor, newColor, t);
            // Material의 색상을 업데이트합니다.
            objLight.color = currentColor;
            // 다음 프레임까지 대기합니다.
            yield return null;
        }

        // 최종 목표 색상을 확실히 적용합니다.
        objLight.color = newColor;
    }
}