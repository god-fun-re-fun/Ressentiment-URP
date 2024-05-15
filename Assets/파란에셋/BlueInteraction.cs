using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueInteraction : MonoBehaviour
{
    public float changeSpeed = 1.0f;

    public Renderer rereHead;
    public Animator animator;

    private bool isColorChanging = false;
    private bool isColorReturning = false;

    public Transform blueSphere;
    private float blueSphereYRotation = 0.0f;

    Color rereColor;
    Color targetRereColor;

    void Start()
    {
        rereColor = rereHead.material.GetColor("_Rim_Color");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            animator.SetBool("isEffecting", true);

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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            targetRereColor = rereColor;
            isColorReturning = true;

            animator.SetBool("isEffecting", false);
        }
    }

    private void Update()
    {
        if (isColorChanging)
        {
            // �����Ͽ� ������ �ε巴�� ����
            rereHead.material.SetColor("_Rim_Color", Color.Lerp(rereHead.material.GetColor("_Rim_Color"), targetRereColor, changeSpeed * Time.deltaTime));

            blueSphereYRotation += 0.1f;
            blueSphere.Rotate(0.0f, blueSphereYRotation, 0.0f);
            // ��ǥ ���� �����ϸ� �� ��ȭ ����
            if (Vector4.Distance(rereHead.material.GetColor("_Rim_Color"), targetRereColor) < 0.01f)
            {
                rereHead.material.SetColor("_Rim_Color", targetRereColor); // ��ǥ ������ ����
                isColorChanging = false; // �� ��ȭ ����
            }
        }
        if (isColorReturning)
        {
            // �����Ͽ� ������ �ε巴�� ����
            rereHead.material.SetColor("_Rim_Color", Color.Lerp(rereHead.material.GetColor("_Rim_Color"), targetRereColor, changeSpeed * Time.deltaTime));

            if (Vector4.Distance(rereHead.material.GetColor("_Rim_Color"), targetRereColor) < 0.01f)
            {
                rereHead.material.SetColor("_Rim_Color", targetRereColor); // ��ǥ ������ ����
                isColorReturning = false; // �� ��ȭ ����                                           
            }
        }
    }
}
