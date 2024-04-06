using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairAppend : MonoBehaviour
{
    [SerializeField]
    Vector3 moveDirection; // ������ �̵� ����
    [SerializeField]
    public float moveSpeed; // �̵� �ӵ�

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            StartCoroutine(MoveWithEscalator(other.gameObject, rb));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(MoveWithEscalator(other.gameObject, other.GetComponent<Rigidbody>()));
            other.GetComponent<Animator>().SetBool("isAppending", false);
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    IEnumerator MoveWithEscalator(GameObject playerObject, Rigidbody rb)
    {
        yield return new WaitForSeconds(1f); // 1�� ��ٸ�
        playerObject.GetComponent<Animator>().SetBool("isAppending", true); // �ȴ� �ִϸ��̼� Ȱ��ȭ
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        // ������Ʈ�� �����÷����Ϳ� ���� �� ��� �̵�
        while (playerObject != null && playerObject.transform.position.y <= this.transform.position.y + this.transform.localScale.y)
        {
            playerObject.transform.Translate(moveSpeed * moveDirection * Time.deltaTime);
            yield return null;
        }

        // �����÷����Ϳ��� ������ ���� ���·� ����
        if (playerObject != null)
        {
            playerObject.GetComponent<Animator>().SetBool("isAppending", false);
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
