using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public Transform mainSpere;
    private float mainSpereYRotation = 0.0f;

    // �� �Լ��� �ٸ� ������Ʈ�� Collider�� �� ������Ʈ�� Trigger�� ���� �� �ڵ����� ȣ��˴ϴ�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            // 'other'�� Ʈ���� ������ ���� ������Ʈ�� Collider�Դϴ�.
            // ���⼭�� �ܼ��� ������� �ϱ� ���� Destroy �Լ��� ����մϴ�.
            Destroy(other.gameObject);

            mainSpereYRotation += 4.0f;
            mainSpere.Rotate(0.0f, mainSpereYRotation, 0.0f);
        }
    }
}
