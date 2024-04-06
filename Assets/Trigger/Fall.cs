using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public Vector3 gravity;

    IEnumerator WaitOneSecondCoroutine(Collider player)
    {
        // 1�� ���� ������ �Ͻ� �����մϴ�.
        yield return new WaitForSeconds(10);

        // ��Ⱑ �Ϸ�� �� setGravityDirection ȣ��
        GravityControll gravityControll = player.GetComponent<GravityControll>();
        if (gravityControll != null) // null üũ �߰�
        {
            gravityControll.setGravityDirection(gravity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Animator>().SetBool("isWatching", true);
            // �ڷ�ƾ ���� �� other�� ���ڷ� ����
            StartCoroutine(WaitOneSecondCoroutine(other));
        }
    }
}
