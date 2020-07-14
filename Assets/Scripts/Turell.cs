using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turell : MonoBehaviour
{
    [SerializeField] private bool FrozenTurrel = false;
    [SerializeField] private GameObject Fort;
    private Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Trigger turell {other.gameObject.name} {other.gameObject.tag}");

        if (other.gameObject.tag == "Post" && !FrozenTurrel && Fort.GetComponent<GoldEnemy>().Gold >= 50 && other.gameObject.GetComponent<TurellSpawn>().CanSpawn) // (&&  && !gameObject.GetComponent<Rigidbody>().isKinematic)
        {
            other.gameObject.GetComponent<TurellSpawn>().PostamentIsActive = true;
            gameObject.GetComponent<Animator>().enabled = true;
            rb.GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("Turell spawn");
        }
        else if (other.gameObject.tag == "Post" && FrozenTurrel && Fort.GetComponent<GoldEnemy>().Gold >= 50 && other.gameObject.GetComponent<TurellSpawn>().CanSpawn)
        {
            other.gameObject.GetComponent<TurellSpawn>().FrozenPostamentIsActive = true;
            gameObject.GetComponent<Animator>().enabled = true;
            rb.GetComponent<Rigidbody>().isKinematic = false;
            Debug.Log("FrozenTurell spawn");
        }
    }

    private void Update()
    {
        if (rb.isKinematic == false) 
        {
            gameObject.GetComponent<Animator>().enabled = true;
        } 
        else if(rb.isKinematic && gameObject.GetComponent<Animator>().enabled) 
        {
            gameObject.GetComponent<Animator>().enabled = false;
        }

    }
}
