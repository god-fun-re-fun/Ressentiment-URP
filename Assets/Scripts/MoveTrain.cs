using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrain : MonoBehaviour
{
    public Transform position1; // ���� ��ġ
    public Transform position2; // �߰� ��ġ
    public Transform position3; // ���� ��ġ
    public float speed = 1.0f; // �̵� �ӵ�
    public float waitTime = 30.0f; // ��ġ 2���� ����ϴ� �ð�(��)

    void Start()
    {
        transform.position = position1.position;
        // ������ �� �̵� �ڷ�ƾ�� �����մϴ�.
        StartCoroutine(MoveBetweenPositions());
    }

    IEnumerator MoveBetweenPositions()
    {
        // ���� ������ �ݺ� �����մϴ�.
        while (true)
        {
            // ��ġ 1���� ��ġ 2�� �̵�
            yield return MoveToPosition(position2.position);
            // ��ġ 2���� ��� ���
            yield return new WaitForSeconds(waitTime);
            // ��ġ 2���� ��ġ 3���� �̵�
            yield return MoveToPosition(position3.position);
            // ��ġ 3���� �ٽ� ��ġ 1�� �̵�
            transform.position = position1.position;
        }
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        // ���� ��ġ���� ��ǥ ��ġ���� �̵��մϴ�.
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }
}
