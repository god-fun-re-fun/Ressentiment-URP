using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class metroposition : MonoBehaviour
{
    public Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }
}
