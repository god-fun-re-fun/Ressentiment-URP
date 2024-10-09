using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControll : MonoBehaviour
{
    private Rigidbody rb;

    // 원하는 방향의 중력을 설정할 수 있습니다.
    // 예를 들어, 아래쪽으로의 중력을 설정하려면 (0, -1, 0)으로 설정합니다.
    public Vector3 gravityDirection = new Vector3(0, -1, 0);

    // 중력의 크기 설정 기본값은 9.81
    private float gravityMagnitude = 9.81f;

    public void setGravityDirection(Vector3 v)
    {
        gravityDirection = v;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        setGravityDirection(gravityDirection);
    }

    void FixedUpdate()
    {
        rb.AddForce(gravityDirection * gravityMagnitude);
    }
}