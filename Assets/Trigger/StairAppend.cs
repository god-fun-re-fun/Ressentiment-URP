using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairAppend : MonoBehaviour
{
    [SerializeField]
    Vector3 moveDirection; // 발판의 이동 방향
    [SerializeField]
    public float moveSpeed; // 이동 속도

    private Dictionary<GameObject, Coroutine> playerCoroutines = new Dictionary<GameObject, Coroutine>(); // 플레이어별 코루틴 관리

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            // 코루틴 시작하고 Dictionary에 저장
            var coroutine = StartCoroutine(MoveWithEscalator(other.gameObject, rb));
            playerCoroutines[other.gameObject] = coroutine;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            // Dictionary에서 해당 플레이어의 코루틴 찾아서 중단
            if (playerCoroutines.TryGetValue(other.gameObject, out Coroutine coroutine))
            {
                StopCoroutine(coroutine);
                playerCoroutines.Remove(other.gameObject); // Dictionary에서 해당 플레이어 제거
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
            // 코루틴이 완료되면 자동으로 플레이어를 Dictionary에서 제거
            playerCoroutines.Remove(playerObject);
        }
    }
}
