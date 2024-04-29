using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // 이 함수는 다른 오브젝트의 Collider가 이 오브젝트의 Trigger에 들어올 때 자동으로 호출됩니다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1") || other.CompareTag("P2") || other.CompareTag("P3"))
        {
            // 'other'는 트리거 영역에 들어온 오브젝트의 Collider입니다.
            // 여기서는 단순히 사라지게 하기 위해 Destroy 함수를 사용합니다.
            Destroy(other.gameObject);
        }
    }
}
