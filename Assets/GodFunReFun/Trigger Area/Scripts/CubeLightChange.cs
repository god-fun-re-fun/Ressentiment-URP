using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeLightChange : MonoBehaviour
{
    public float changeSpeed = 5.0f; // ���� ��ȭ�� �ɸ��� �ð� (��)

    private Material glassMaterial; // ������Ʈ�� Material

    private bool isColorChanging = false;
    private bool isColorReturning = false;

    Color glassColor;

    Color targetGlassColor;

    void Start()
    {
        glassMaterial = GetComponent<Renderer>().material;

        glassColor = glassMaterial.GetColor("_EmissionColor");
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

            isColorChanging = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            targetGlassColor = glassColor;

            isColorReturning = true;
        }
    }


    private void Update()
    {
        if (isColorChanging)
        {
            // �����Ͽ� ������ �ε巴�� ����
            glassMaterial.SetColor("_EmissionColor", Color.Lerp(glassMaterial.GetColor("_EmissionColor"), targetGlassColor, changeSpeed * Time.deltaTime));
            
            // ��ǥ ���� �����ϸ� �� ��ȭ ����
            if (Vector4.Distance(glassMaterial.color, targetGlassColor) < 0.01f)
            {
                glassMaterial.color = targetGlassColor; // ��ǥ ������ ����
                isColorChanging = false; // �� ��ȭ ����
            }
        }
        if (isColorReturning)
        {
            // �����Ͽ� ������ �ε巴�� ����
            glassMaterial.SetColor("_EmissionColor", Color.Lerp(glassMaterial.GetColor("_EmissionColor"), targetGlassColor, (changeSpeed / 5) * Time.deltaTime));
            
            // ��ǥ ���� �����ϸ� �� ��ȭ ����
            if (Vector4.Distance(glassMaterial.color, targetGlassColor) < 0.01f)
            {
                glassMaterial.color = targetGlassColor; // ��ǥ ������ ����
                isColorReturning = false; // �� ��ȭ ����
            }
        }
    }
}
