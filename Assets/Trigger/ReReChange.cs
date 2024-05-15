using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReReChange : MonoBehaviour
{
    public float changeSpeed = 5.0f; // ���� ��ȭ�� �ɸ��� �ð� (��)

    public Renderer sphere; //�ٲ�� ��

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
            // �����Ͽ� ������ �ε巴�� ����
            sphere.material.SetColor("_Rim_Color", Color.Lerp(sphere.material.GetColor("_Rim_Color"), targetRereColor, changeSpeed * Time.deltaTime));

            // ��ǥ ���� �����ϸ� �� ��ȭ ����
            if (Vector4.Distance(sphere.material.GetColor("_Rim_Color"), targetRereColor) < 0.01f)
            {
                sphere.material.SetColor("_Rim_Color", targetRereColor); // ��ǥ ������ ����
                isColorChanging = false; // �� ��ȭ ����
            }
        }
        if (isColorReturning)
        {
            // �����Ͽ� ������ �ε巴�� ����
            sphere.material.SetColor("_Rim_Color", Color.Lerp(sphere.material.GetColor("_Rim_Color"), targetRereColor, changeSpeed * Time.deltaTime));
            
            if (Vector4.Distance(sphere.material.GetColor("_Rim_Color"), targetRereColor) < 0.01f)
            {
                sphere.material.SetColor("_Rim_Color", targetRereColor); // ��ǥ ������ ����
                isColorReturning = false; // �� ��ȭ ����                                           
            }
        }
    }
}
