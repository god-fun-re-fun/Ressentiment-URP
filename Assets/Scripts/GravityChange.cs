using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChange : MonoBehaviour
{
    public Vector3 gravity;

    private void OnTriggerEnter(Collider other)
    {
        GravityControll gravityControll = other.GetComponent<GravityControll>();
        gravityControll.setGravityDirection(gravity);
    }
}
