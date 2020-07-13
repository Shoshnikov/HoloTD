using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Hp;
    GameObject Fort;
    public bool frozen;
    void Start()
    {
        Hp = 3;
        Fort = GameObject.Find("Fort");
    }

    void Update()
    {
        if (Hp <= 0)
        {
            Destroy(gameObject);
            Fort.GetComponent<GoldEnemy>().Gold += 10;
        }
    }
}
