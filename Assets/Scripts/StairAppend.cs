using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairAppend : MonoBehaviour
{
    [SerializeField]
    Vector3 moveDirection; // 발판의 이동 방향
    [SerializeField]
    public float moveSpeed; // 이동 속도

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
        yield return new WaitForSeconds(1f); // 1초 기다림
        playerObject.GetComponent<Animator>().SetBool("isAppending", true); // 걷는 애니메이션 활성화
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        // 오브젝트가 에스컬레이터에 있을 때 계속 이동
        while (playerObject != null && playerObject.transform.position.y <= this.transform.position.y + this.transform.localScale.y)
        {
            playerObject.transform.Translate(moveSpeed * moveDirection * Time.deltaTime);
            yield return null;
        }

        // 에스컬레이터에서 내리면 원래 상태로 복구
        if (playerObject != null)
        {
            playerObject.GetComponent<Animator>().SetBool("isAppending", false);
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
