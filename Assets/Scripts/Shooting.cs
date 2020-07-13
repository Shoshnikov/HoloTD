using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Vector3 TargetPosition;
    bool TargetIsSpoting;
    int TargetID;
    Vector3 Direction;
    public GameObject Target;
    public GameObject BulletPrototype;
    Vector3 BulletPosition;
    GameObject Bullet;
    float Delay;
    void Start()
    {
        TargetIsSpoting = false;
        Delay = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !TargetIsSpoting && Target != null)
        {
            Target = other.gameObject;
            TargetPosition = other.gameObject.GetComponent<Transform>().position;
            TargetID = other.GetInstanceID();
            Direction = (other.transform.position - transform.position).normalized;
            TargetIsSpoting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetInstanceID() == TargetID)
            TargetIsSpoting = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!TargetIsSpoting)
            Target = other.gameObject;
    }

    void Update()
    {
        if (Target == null)
            TargetIsSpoting = false;

        Delay += Time.deltaTime;
        if(Target != null && TargetIsSpoting && Delay >= 1)
        {
            BulletPosition = transform.GetChild(0).transform.position;
            Bullet = Instantiate(BulletPrototype, BulletPosition, new Quaternion(0, 0, 0, 0));
            Bullet.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            Bullet.transform.parent = transform;
            Delay = 0;
        }

    }
}
