using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
    public float amplitude = 0.5f; // 움직임의 크기
    public float frequency = 1f; // 움직임의 속도

    // 시작 위치를 저장할 변수
    private Vector3 startPos;

    void Start()
    {
        // 오브젝트의 시작 위치를 저장
        startPos = transform.position;
    }

    void Update()
    {
        // 시간에 따라 변하는 값 생성
        float tempPos = amplitude * Mathf.Sin(Time.time * frequency);

        // 오브젝트의 위치를 상하로 움직임
        transform.position = new Vector3(startPos.x, startPos.y + tempPos, startPos.z);
    }
}