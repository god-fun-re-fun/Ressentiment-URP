using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityChange : MonoBehaviour
{
    public Vector3 gravity;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("P1") || other.CompareTag("P2"))
        {
            GravityControll gravityControll = other.GetComponent<GravityControll>();
            gravityControll.setGravityDirection(gravity);
        }
    }
}
