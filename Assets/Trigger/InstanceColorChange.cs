using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceColorChange : MonoBehaviour
{
    public float changeSpeed = 5.0f; // ���� ��ȭ�� �ɸ��� �ð� (��)

    private Material glassMaterial; // ������Ʈ�� Material

    public Light light;

    private bool isColorChanging = false;
    private bool isColorReturning = false;

    Color glassColor;
    Color lightColor;

    Color targetGlassColor;
    Color targetLightColor;

    void Start()
    {
        glassMaterial = GetComponent<Renderer>().material;

        glassColor = glassMaterial.GetColor("_EmissionColor");

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

            targetGlassColor = waterRenderer.material.GetColor("_TopColor");
            targetLightColor = waterRenderer.material.GetColor("_TopColor");

            isColorChanging = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            targetGlassColor = glassColor;
            targetLightColor = lightColor;

            isColorReturning = true;
        }
    }
    

    private void Update()
    {
        if (isColorChanging)
        {
            // �����Ͽ� ������ �ε巴�� ����
            glassMaterial.SetColor("_EmissionColor", Color.Lerp(glassMaterial.GetColor("_EmissionColor"), targetGlassColor, changeSpeed * Time.deltaTime));
            light.color = Color.Lerp(light.color, targetLightColor, changeSpeed * Time.deltaTime);

            // ��ǥ ���� �����ϸ� �� ��ȭ ����
            if (Vector4.Distance(glassMaterial.color, targetGlassColor) < 0.01f)
            {
                glassMaterial.color = targetGlassColor; // ��ǥ ������ ����
                light.color = targetLightColor; // ��ǥ ������ ����
                isColorChanging = false; // �� ��ȭ ����
            }
        }
        if (isColorReturning)
        {
            // �����Ͽ� ������ �ε巴�� ����
            glassMaterial.SetColor("_EmissionColor", Color.Lerp(glassMaterial.GetColor("_EmissionColor"), targetGlassColor, (changeSpeed / 2) * Time.deltaTime));
            light.color = Color.Lerp(light.color, targetLightColor, changeSpeed * Time.deltaTime);

            // ��ǥ ���� �����ϸ� �� ��ȭ ����
            if (Vector4.Distance(glassMaterial.color, targetGlassColor) < 0.01f)
            {
                glassMaterial.color = targetGlassColor; // ��ǥ ������ ����
                light.color = targetLightColor; // ��ǥ ������ ����
                isColorReturning = false; // �� ��ȭ ����
            }
        }
    }
}
