using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedMovement : MonoBehaviour
{
    [SerializeField]
    public GameObject[] points;
    private int currentPoint = 0;
    public float speed = 1f;



    void Start()
    {
        //points = GameObject.FindGameObjectsWithTag("Point");
        transform.LookAt(points[currentPoint].transform);
    }

    void Update()
    {
        if (Vector3.Distance(points[currentPoint].transform.position, transform.position) <= 0.000001f)
        {
            if (currentPoint == 3)
                Destroy(this.gameObject);
            currentPoint = ++currentPoint % points.Length;
        }
        transform.LookAt(points[currentPoint].transform);
        transform.Translate(transform.TransformDirection(Vector3.forward * speed), points[currentPoint].transform);
        Debug.Log(currentPoint);
    }

}
