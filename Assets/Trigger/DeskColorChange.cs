using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeskColorChange : MonoBehaviour
{
    public float changeSpeed = 5.0f; // ���� ��ȭ�� �ɸ��� �ð� (��)

    private Material screenMaterial; // ������Ʈ�� Material

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
            // �����Ͽ� ������ �ε巴�� ����
            screenMaterial.SetColor("_EmissionColor", Color.Lerp(screenMaterial.GetColor("_EmissionColor"), targetScreenColor, changeSpeed * Time.deltaTime));

            // ��ǥ ���� �����ϸ� �� ��ȭ ����
            if (Vector4.Distance(screenMaterial.color, targetScreenColor) < 0.01f)
            {
                screenMaterial.color = targetScreenColor; // ��ǥ ������ ����
                isColorChanging = false; // �� ��ȭ ����
            }
        }
        if (isColorReturning)
        {
            // �����Ͽ� ������ �ε巴�� ����
            screenMaterial.SetColor("_EmissionColor", Color.Lerp(screenMaterial.GetColor("_EmissionColor"), targetScreenColor, (changeSpeed/4) * Time.deltaTime));

            // ��ǥ ���� �����ϸ� �� ��ȭ ����
            if (Vector4.Distance(screenMaterial.color, targetScreenColor) < 0.01f)
            {
                screenMaterial.color = targetScreenColor; // ��ǥ ������ ����
                isColorReturning = false; // �� ��ȭ ����
            }
        }
    }
}
