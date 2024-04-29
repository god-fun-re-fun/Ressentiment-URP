using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateY : MonoBehaviour
{
    [SerializeField]
    float Y;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1") || other.CompareTag("P2") || other.CompareTag("P3"))
        {
            // ���� ����, ��ǥ ����, ȸ���� �ɸ��� �ð��� ���ڷ� �����մϴ�.
            StartCoroutine(RotateOverTime(other.transform, Y, 0.1f));
        }
    }

    // �ڷ�ƾ�� �����մϴ�. target�� ȸ����ų ���, angle�� ȸ���� ����, duration�� ȸ���� �ɸ��� �ð��Դϴ�.
    IEnumerator RotateOverTime(Transform target, float angle, float duration)
    {
        yield return new WaitForSeconds(2f); // 2�� ���

        Quaternion originalRotation = target.rotation; // ���� ȸ������ �����մϴ�.
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0, angle, 0); // ��ǥ ȸ������ ����մϴ�.

        float elapsedTime = 0; // ��� �ð��� �����մϴ�.

        while (elapsedTime < duration)
        {
            // ��� �ð��� �������� ���� ȸ������ ����մϴ�. Lerp �Լ��� �� ȸ���� ���̸� �ε巴�� �����մϴ�.
            target.rotation = Quaternion.Lerp(originalRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // ��� �ð��� ������Ʈ�մϴ�.
            yield return null; // ���� �����ӱ��� ����մϴ�.
        }

        // ���� ��ǥ ȸ������ �����մϴ�. �̴� ��Ȯ�� ȸ������ �����ϱ� �����Դϴ�.
        target.rotation = targetRotation;
    }
}