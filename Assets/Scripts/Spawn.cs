using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public bool flag = false;
    public GameObject enemy;
    private GameObject spawnedEnemy;
    float time;
    public int ID;
    System.Random Rnd;
    int Delay;
    GameObject Fort;
    int MaxDelay;
    int MinDelay;
    public GameObject[] points;

    private void Awake()
    {
        //определяется точка спавна
        if (gameObject.name == "Respawn")
            ID = 0;
        if (gameObject.name == "Respawn1")
            ID = 1;
        Fort = GameObject.Find("Fort");
    }

    void Start()
    {
        MaxDelay = 10;
        MinDelay = 5;
        time = 0;
        Rnd = new System.Random();
        Delay = Rnd.Next(MinDelay, MaxDelay);
    }

    void Update()
    {
        if (flag)
        {
            //спавн крипов пока с задержкой в две секунды
            time += Time.deltaTime;
            if (time >= Delay && Fort.GetComponent<GoldEnemy>().EnemyRemain > 0)
            {
                Instantiate(enemy, transform.position, transform.rotation, transform);
                //spawnedEnemy.GetComponent<AdvancedMovement>().points = points;
                time = 0;
                Delay = Rnd.Next(MinDelay, MaxDelay);
                Fort.GetComponent<GoldEnemy>().EnemyRemain--;
            }

            switch (Fort.GetComponent<GoldEnemy>().EnemyRemain)
            {
                case 80:
                    MaxDelay = 9;
                    MinDelay = 4;
                    break;

                case 50:
                    MaxDelay = 8;
                    MinDelay = 3;
                    break;

                case 30:
                    MaxDelay = 7;
                    MinDelay = 2;
                    break;
                case 10:
                    MaxDelay = 6;
                    MinDelay = 1;
                    break;

            }
        }
    }
}
