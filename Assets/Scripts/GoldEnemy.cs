using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoldEnemy : MonoBehaviour
{
    int OriginalGold;
    int OriginalEnemyCount;
    int OriginalHp;
    public int Gold;
    public int EnemyRemain;
    public int Hp;
    //public Text GoldText;
    //public Text EnemyText;
    //public Slider HpSlider;


    void Start()
    {
        OriginalGold = 100;
        OriginalEnemyCount = 100;
        Gold = 100;
        EnemyRemain = 100;
        OriginalHp = 100;
        Hp = 100;
    }

    private void Update()
    {
        //if (OriginalEnemyCount != EnemyRemain)
        //{
        //    EnemyText.text = "Enemy remain: " + EnemyRemain;
        //    OriginalEnemyCount = EnemyRemain;
        //}

        //if (OriginalGold != Gold)
        //{
        //    GoldText.text = "Gold: " + Gold;
        //    OriginalGold = Gold;
        //}

        //if (OriginalHp != Hp)
        //{
        //    HpSlider.value = Hp;
        //    OriginalHp = Hp;
        //}

        if(Hp <= 0)
        {
            SceneManager.LoadScene("GameScene");
        }

    }
}
