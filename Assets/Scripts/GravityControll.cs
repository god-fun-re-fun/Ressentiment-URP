using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityControll : MonoBehaviour
{
    // Rigidbody ������Ʈ�� ������ ����
    private Rigidbody rb;
    // ���ϴ� ������ �߷��� ������ �� �ֽ��ϴ�.
    // ���� ���, �Ʒ��������� �߷��� �����Ϸ��� (0, -1, 0)���� �����մϴ�.
    public Vector3 gravityDirection = new Vector3(0,-1,0);

    public void setGravityDirection(Vector3 v)
    {
        gravityDirection = v;
    }

    void Start()
    {
        // Rigidbody ������Ʈ ��������
        rb = GetComponent<Rigidbody>();
        setGravityDirection(gravityDirection);
    }

    // �߷��� ũ�� ���� (�⺻���� 9.81 * 2 = 19.62�� �����Ϸ��� ���ϴ� ���� �����մϴ�.)
    float gravityMagnitude = 9.81f;

    private void FixedUpdate()
    {
        rb.AddForce(gravityDirection * gravityMagnitude);
    }
}
