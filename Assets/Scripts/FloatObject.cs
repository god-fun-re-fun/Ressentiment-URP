using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
    public float amplitude = 0.5f; // �������� ũ��
    public float frequency = 1f; // �������� �ӵ�

    // ���� ��ġ�� ������ ����
    private Vector3 startPos;

    void Start()
    {
        // ������Ʈ�� ���� ��ġ�� ����
        startPos = transform.position;
    }

    void Update()
    {
        // �ð��� ���� ���ϴ� �� ����
        float tempPos = amplitude * Mathf.Sin(Time.time * frequency);

        // ������Ʈ�� ��ġ�� ���Ϸ� ������
        transform.position = new Vector3(startPos.x, startPos.y + tempPos, startPos.z);
    }
}
