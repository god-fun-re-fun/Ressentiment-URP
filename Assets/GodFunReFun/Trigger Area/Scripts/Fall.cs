using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    public Vector3 gravity;
    public float standingTime;

    IEnumerator WaitOneSecondCoroutine(Collider player)
    {
        // 1초 동안 실행을 일시 중지합니다.
        yield return new WaitForSeconds(standingTime);

        // 대기가 완료된 후 setGravityDirection 호출
        GravityControll gravityControll = player.GetComponent<GravityControll>();
        if (gravityControll != null) // null 체크 추가
        {
            gravityControll.setGravityDirection(gravity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            other.GetComponent<Animator>().SetBool("isWatching", true);
            // 코루틴 시작 시 other를 인자로 전달
            StartCoroutine(WaitOneSecondCoroutine(other));
        }
    }
}
