using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControll : MonoBehaviour
{
    // Rigidbody 컴포넌트를 저장할 변수
    private Rigidbody rb;
    // 원하는 방향의 중력을 설정할 수 있습니다.
    // 예를 들어, 아래쪽으로의 중력을 설정하려면 (0, -1, 0)으로 설정합니다.
    public Vector3 gravityDirection = new Vector3(0,-1,0);

    public void setGravityDirection(Vector3 v)
    {
        gravityDirection = v;
    }

    void Start()
    {
        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
        setGravityDirection(gravityDirection);
    }

    // 중력의 크기 설정 (기본값인 9.81 * 2 = 19.62을 변경하려면 원하는 값을 설정합니다.)
    float gravityMagnitude = 9.81f;

    private void FixedUpdate()
    {
        rb.AddForce(gravityDirection * gravityMagnitude);
    }
}
