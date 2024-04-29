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
            // 시작 각도, 목표 각도, 회전에 걸리는 시간을 인자로 전달합니다.
            StartCoroutine(RotateOverTime(other.transform, Y, 0.1f));
        }
    }

    // 코루틴을 정의합니다. target은 회전시킬 대상, angle은 회전할 각도, duration은 회전에 걸리는 시간입니다.
    IEnumerator RotateOverTime(Transform target, float angle, float duration)
    {
        yield return new WaitForSeconds(2f); // 2초 대기

        Quaternion originalRotation = target.rotation; // 시작 회전값을 저장합니다.
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0, angle, 0); // 목표 회전값을 계산합니다.

        float elapsedTime = 0; // 경과 시간을 추적합니다.

        while (elapsedTime < duration)
        {
            // 경과 시간을 기준으로 현재 회전값을 계산합니다. Lerp 함수는 두 회전값 사이를 부드럽게 보간합니다.
            target.rotation = Quaternion.Lerp(originalRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // 경과 시간을 업데이트합니다.
            yield return null; // 다음 프레임까지 대기합니다.
        }

        // 최종 목표 회전값을 적용합니다. 이는 정확한 회전값을 보장하기 위함입니다.
        target.rotation = targetRotation;
    }
}