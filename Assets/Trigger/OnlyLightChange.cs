using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyLightChange : MonoBehaviour
{
    public float changeSpeed = 0.05f; // ���� ��ȭ�� �ɸ��� �ð� (��)

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
            // �����Ͽ� ������ �ε巴�� ����
            light.color = Color.Lerp(light.color, targetLightColor, changeSpeed * Time.deltaTime);

            // ��ǥ ���� �����ϸ� �� ��ȭ ����
            if (Vector4.Distance(light.color, targetLightColor) < 0.01f)
            {
                light.color = targetLightColor; // ��ǥ ������ ����
                isColorChanging = false; // �� ��ȭ ����
            }
        }
        if (isColorReturning)
        {
            // �����Ͽ� ������ �ε巴�� ����
            light.color = Color.Lerp(light.color, targetLightColor, changeSpeed * Time.deltaTime);

            // ��ǥ ���� �����ϸ� �� ��ȭ ����
            if (Vector4.Distance(light.color, targetLightColor) < 0.01f)
            {
                light.color = targetLightColor; // ��ǥ ������ ����
                isColorReturning = false; // �� ��ȭ ����
            }
        }
    }
}
