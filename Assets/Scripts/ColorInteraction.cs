using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorInteraction : MonoBehaviour
{
    public float changeDuration = 2.0f; // ���� ��ȭ�� �ɸ��� �ð� (��)

    private Material objMaterial; // ������Ʈ�� Material

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
        // ���� ������ Material�� ���� �������� �����մϴ�.
        Color startColor = objColor;
        // ��ȭ�� �����ϱ� ���� �ð��� ����մϴ�.
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            // ��ȭ ���� ������ ����մϴ�. (0���� 1 ������ ��)
            float t = (Time.time - startTime) / duration;
            // ���� ������ ����մϴ�.
            Color currentColor = Color.Lerp(startColor, newColor, t);
            // Material�� ������ ������Ʈ�մϴ�.
            objMaterial.SetColor("_EmissionColor", currentColor);
            // ���� �����ӱ��� ����մϴ�.
            yield return null;
        }

        // ���� ��ǥ ������ Ȯ���� �����մϴ�.
        objMaterial.SetColor("_EmissionColor", newColor);
        rere.GetComponent<Animator>().SetBool("isEffecting", false);
    }

    IEnumerator ChangeLight(Color lighttColor, Color newColor, float duration)
    {
        // ���� ������ Material�� ���� �������� �����մϴ�.
        Color startColor = lighttColor;
        // ��ȭ�� �����ϱ� ���� �ð��� ����մϴ�.
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            // ��ȭ ���� ������ ����մϴ�. (0���� 1 ������ ��)
            float t = (Time.time - startTime) / duration;
            // ���� ������ ����մϴ�.
            Color currentColor = Color.Lerp(startColor, newColor, t);
            // Material�� ������ ������Ʈ�մϴ�.
            objLight.color = currentColor;
            // ���� �����ӱ��� ����մϴ�.
            yield return null;
        }

        // ���� ��ǥ ������ Ȯ���� �����մϴ�.
        objLight.color = newColor;
    }
}