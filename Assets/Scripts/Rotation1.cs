using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation1 : MonoBehaviour
{
    private GameObject target;
    private Vector3 targetPosition;
    private const float speed = 50f;

    void Start()
    {

    }

    void Update()
    {
        if (target != null && targetPosition != target.transform.position) 
            transform.LookAt(target.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy") 
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy") 
        {
            target = null;
        }
            
    }
}
