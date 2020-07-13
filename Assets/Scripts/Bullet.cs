using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : MonoBehaviour
{
    public enum BulletType {Fireboll, FrozenBoll}
    public BulletType type = BulletType.Fireboll;
    public GameObject Target;
    [SerializeField]
    private float speed = 1;



    void Update()
    {
        if (Target == null)
            Destroy(gameObject);
        else 
            transform.position = Vector3.MoveTowards(transform.position,Target.transform.position, 0.003f);


        if (Target != null && Vector3.Distance(transform.position, Target.transform.position) < 0.001f)
        {
            if (type == BulletType.Fireboll)
            {
                Target.GetComponent<Enemy>().Hp--;
            }
            else if(Target.GetComponent<Movment>().speed > 0.0005f)
            {
                Target.GetComponent<Movment>().speed /= 2;
                Target.GetComponent<Enemy>().frozen = true;
            }
            Destroy(this.gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("Enemy hited");
    //        Target.GetComponent<Enemy>().Hp--;
    //        Destroy(this.gameObject);
    //    }

    //}
}
