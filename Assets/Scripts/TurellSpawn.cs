using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurellSpawn : MonoBehaviour
{
    [SerializeField] private GameObject TurellPref;
    [SerializeField] private GameObject FrozenTurellPref;

    public bool CanSpawn;
    public bool PostamentIsActive;
    public bool FrozenPostamentIsActive;
    GameObject Turell;
    GameObject FrozenTurell;
    bool TurellIsSpawned;
    bool FrozenTurellIsSpawned;
    GameObject Fort;

    void Start()
    {
        CanSpawn = true;
        PostamentIsActive = false;
        TurellIsSpawned = false;
        FrozenPostamentIsActive = false;
        FrozenTurellIsSpawned = false;
        Fort = GameObject.Find("Fort");
    }

    void Update()
    {
        //спавн турели, в будущем по нажатию
        if (PostamentIsActive && !TurellIsSpawned && CanSpawn && Fort.GetComponent<GoldEnemy>().Gold >= 50)
        {
            Turell = Instantiate(TurellPref, transform.position - new Vector3(0,0.5f), transform.rotation);
            //Turell = PhotonNetwork.Instantiate("Deer_tower", transform.position - new Vector3(0, 1), transform.rotation);
            Turell.transform.parent = gameObject.transform;
            Turell.transform.localScale = new Vector3(10f,50f,10f);
            TurellIsSpawned = true;
            Fort.GetComponent<GoldEnemy>().Gold -= 50;
            CanSpawn = false;
        }
        else if (FrozenPostamentIsActive && !FrozenTurellIsSpawned && CanSpawn)
        {
            FrozenTurell = Instantiate(FrozenTurellPref, transform.position - new Vector3(0, 0.5f), transform.rotation);
            FrozenTurell.transform.parent = gameObject.transform;
            FrozenTurell.transform.localScale = new Vector3(10f, 50f, 10f);
            FrozenTurellIsSpawned = true;
            Fort.GetComponent<GoldEnemy>().Gold -= 50;
            CanSpawn = false;
        }

        //"анимация" строительства турели
        if (TurellIsSpawned && Turell.transform.position.y < transform.position.y)//1.125
            Turell.transform.position = new Vector3(Turell.transform.position.x, Turell.transform.position.y + 0.4f* Time.deltaTime, Turell.transform.position.z);

        else if (FrozenTurellIsSpawned && FrozenTurell.transform.position.y < transform.position.y)//1.125
            FrozenTurell.transform.position = new Vector3(FrozenTurell.transform.position.x, FrozenTurell.transform.position.y + 0.4f * Time.deltaTime, FrozenTurell.transform.position.z);
    }

    /*void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Deer_tower_Model")
        {
            PostamentIsActive = true;
            Debug.Log("Башня ударилась о постамент");
            col.gameObject.GetComponent<Animator>().enabled = true;
        }
        else if (col.gameObject.name == "Freeze_tower_Model")
        {
            FrozenPostamentIsActive = true;
            Debug.Log("Башня ударилась о постамент");
            col.gameObject.GetComponent<Animator>().enabled = true;
        }
    }*/

}
