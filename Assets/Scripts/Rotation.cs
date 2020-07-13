using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    Vector3 TargetPosition;
    GameObject Target;
    Vector3 Direction;
    Quaternion Look;
    public float X, Y, Z;
    string PostamentName;
    void Start()
    {
        //определяем постамент, на котором заспавнена турель (это важно для вектора в LookRotation, ну да, имеется небольшой костыль)
        PostamentName = transform.parent.parent.name;
        switch (PostamentName)
        {
            case "Postament":
                X = -1; Y = 0; Z = 0.1f;
                break;

            case "Postament (1)":
                X = 1; Y = 0; Z = 0.1f;
                break;

            case "Postament (2)":
                X = 1; Y = 0; Z = 0.1f;
                break;

            case "Postament (3)":
                X = -1; Y = 0; Z = 0.1f;
                break;

            case "Postament (4)":
                X = -1; Y = 0; Z = 0.1f;
                break;

            case "Postament (5)":
                X = 0.1f; Y = 0; Z = -1;
                break;

            case "Postament (6)":
                X = 1; Y = 0; Z = 0.1f;
                break;

            case "Postament (7)":
                X = -1; Y = 0; Z = 0.1f;
                break;
        }

    }

    void Update()
    {
        //берем цель и позицию, по которой стреляем у турелли, если цель является крипом
        //if(Target!= null)
            if (GetComponentInChildren<Shooting>().Target.gameObject.tag == "Enemy")
                Target = GetComponentInChildren<Shooting>().Target;
        TargetPosition = GetComponentInChildren<Shooting>().TargetPosition;

        //если цель еще существует и поменяла позицию, то высчитывается разница, на которую нужно повернуть турель
        if (Target != null && Target.transform.position != TargetPosition)
        {
            Direction = (transform.position - Target.transform.position).normalized;
            Direction.y = 0;// потому что по у оно не крутится
            Look = Quaternion.LookRotation(Direction, new Vector3(X, Y, Z));//   -1, 0, 0.1f - запомните, твари!  Это костыль, оно работает, а ты ему не мешай =)
            transform.rotation = Quaternion.Lerp(transform.rotation, Look, 0.5f);
            TargetPosition = Target.transform.position;
        }
    }
}
