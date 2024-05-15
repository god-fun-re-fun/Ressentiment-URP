using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReReAction : MonoBehaviour
{
    public GameObject ReRe;
    private Animator _animator;

    private void Start()
    {
        _animator = ReRe.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1"))
        {
            _animator.SetBool("isEffecting", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _animator)
        {
            _animator.SetBool("isEffecting", false);
        }
    }
}
