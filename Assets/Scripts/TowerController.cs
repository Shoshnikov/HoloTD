using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField]
    private GameObject Target;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletPosition;

    private GameObject bullet;



    void Start()
    {
        StartCoroutine(ShootingLoop());
    }

    void Update()
    {
        if (Target)
            transform.LookAt(Target.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Target && other.tag == "Enemy")
            Target = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Target)
            Target = null;
    }


    private void OnTriggerStay(Collider other)
    {
        if (!Target && other.tag == "Enemy")
            Target = other.gameObject;
    }

    private IEnumerator ShootingLoop() 
    {

        while (true) 
        {
            if (Target) 
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
                bullet.GetComponent<Bullet>().Target = Target;
            }
            yield return new WaitForSeconds(1);
        }

    }


}
