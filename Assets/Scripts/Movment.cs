using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movment : MonoBehaviour
{
    Vector3 point;
    Vector3 point1;
    Vector3 point2;
    Vector3 point3;
    Vector3 point4;
    Vector3 point5;
    Vector3 Fort;
    GameObject fort;
    int SpawnID;
    enum Targets{point,point1,point2,point3,point4,point5,Fort}
    Targets target;
    public float speed;
    void Start()
    {
        //определение маршрута в зависимости от точки спавна
        SpawnID = gameObject.GetComponentInParent<Spawn>().ID;
        if(SpawnID == 0)
        {
            point = GameObject.Find("Point").transform.position;
            point1 = GameObject.Find("Point (1)").transform.position;
            point2 = GameObject.Find("Point (2)").transform.position;
            target = Targets.point;
        }
        else if (SpawnID == 1)
        {
            point3 = GameObject.Find("Point (3)").transform.position;
            point4 = GameObject.Find("Point (4)").transform.position;
            point5 = GameObject.Find("Point (5)").transform.position;
            target = Targets.point3;
            gameObject.transform.rotation = new Quaternion(transform.rotation.x, 1f, transform.rotation.z, 0f);
        }
        Fort = GameObject.Find("Fort").transform.position;
        fort = GameObject.Find("Fort");
        speed = 0.01f;
    }

    void Update()
    {
        //первый маршрут
        if(SpawnID == 0)
        {
            if (transform.position.z < point.z && target == Targets.point)
                transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
            if (transform.position == point)
            {
                target = Targets.point1;
                gameObject.transform.rotation = new Quaternion(transform.rotation.x, -1f, transform.rotation.z, 1f);
            }

            if (transform.position.x > point1.x && target == Targets.point1)
                transform.position = Vector3.MoveTowards(transform.position, point1, speed * Time.deltaTime);
            if (transform.position == point1)
            {
                target = Targets.point2;
                gameObject.transform.rotation = new Quaternion(transform.rotation.x, 0f, transform.rotation.z, 1f);
            }

            if (transform.position.z < point2.z && target == Targets.point2)
                transform.position = Vector3.MoveTowards(transform.position, point2, speed * Time.deltaTime);
            if (transform.position == point2)
            {
                target = Targets.Fort;
                gameObject.transform.rotation = new Quaternion(transform.rotation.x, -1f, transform.rotation.z, 1f);
            }

            if (transform.position.x > Fort.x && target == Targets.Fort)
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y);
            if (transform.position.x <= Fort.x && target == Targets.Fort)
            {
                fort.GetComponent<GoldEnemy>().Hp -=10;
                Destroy(gameObject);
            }
        }
        //второй маршрут
        else if (SpawnID == 1)
        {
            if (transform.position.z > point3.z && target == Targets.point3)
                transform.position = Vector3.MoveTowards(transform.position,point3, speed * Time.deltaTime);
            if (transform.position == point3)
            {
                target = Targets.point4;
                gameObject.transform.rotation = new Quaternion(transform.rotation.x, 1f, transform.rotation.z, 1f);
            }

            if (transform.position.x < point4.x && target == Targets.point4)
                transform.position = Vector3.MoveTowards(transform.position, point4, speed * Time.deltaTime);
            if (transform.position == point4)
            {
                target = Targets.point5;
                gameObject.transform.rotation = new Quaternion(transform.rotation.x, 1f, transform.rotation.z, 0f);
            }

            if(transform.position.z > point5.z && target == Targets.point5)
                transform.position = Vector3.MoveTowards(transform.position, point5, speed * Time.deltaTime);
            if (transform.position == point5)
            {
                target = Targets.Fort;
                gameObject.transform.rotation = new Quaternion(transform.rotation.x, 1f, transform.rotation.z, 1f);
            }

            if (transform.position.x < Fort.x && target == Targets.Fort)
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
            if (transform.position.x >= Fort.x && target == Targets.Fort)
            {
                fort.GetComponent<GoldEnemy>().Hp -= 10;
                Destroy(gameObject);
            }
        }
    }

}
