using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairAppend : MonoBehaviour
{
    [SerializeField]
    Vector3 moveDirection; // ������ �̵� ����
    [SerializeField]
    public float moveSpeed; // �̵� �ӵ�

    private Dictionary<GameObject, Coroutine> playerCoroutines = new Dictionary<GameObject, Coroutine>(); // �÷��̾ �ڷ�ƾ ����

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            // �ڷ�ƾ �����ϰ� Dictionary�� ����
            var coroutine = StartCoroutine(MoveWithEscalator(other.gameObject, rb));
            playerCoroutines[other.gameObject] = coroutine;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            // Dictionary���� �ش� �÷��̾��� �ڷ�ƾ ã�Ƽ� �ߴ�
            if (playerCoroutines.TryGetValue(other.gameObject, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
                playerCoroutines.Remove(other.gameObject); // Dictionary���� �ش� �÷��̾� ����
            }

            Animator animator = other.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("isAppending", false);
            }

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    IEnumerator MoveWithEscalator(GameObject playerObject, Rigidbody rb)
    {
        yield return new WaitForSeconds(1f);
        Animator animator = playerObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isAppending", true);
        }

        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        while (playerObject != null && playerObject.transform.position.y <= this.transform.position.y + this.transform.localScale.y)
        {
            playerObject.transform.Translate(moveSpeed * moveDirection * Time.deltaTime);
            yield return null;
        }

        if (playerObject != null && animator != null)
        {
            animator.SetBool("isAppending", false);
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            // �ڷ�ƾ�� �Ϸ�Ǹ� �ڵ����� �÷��̾ Dictionary���� ����
            playerCoroutines.Remove(playerObject);
        }
    }
}
