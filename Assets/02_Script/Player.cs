using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData Pdata;

    GameObject[] Monster;
    Rigidbody2D rig;

    public bool Back = false;
    public Vector3 OriPos;
    Animator ani;

    public bool home = true;

    //마법진
    public GameObject MagicAura;
    public Transform T_MagicAura;
    // 마법 공격
    public GameObject Explosion;

    private void Start()
    {
        Monster = GameObject.FindGameObjectsWithTag("Monster");
        rig = GetComponent<Rigidbody2D>();
        OriPos = transform.position;
        ani = GetComponent<Animator>();
    }

    public void NomalAttack()
    {
        if(GameManager.ins.CurrTurn == false && home)
        {
            StartCoroutine("NomalAttackCT");
        }
    }

    public void Damage(int Attack)
    {
        Pdata.Hp -= Attack;
        ani.SetTrigger("damage");

        if(Pdata.Hp <= 0)
        {
            GameManager.ins.D_Player.Remove(Pdata.Job);
            Destroy(gameObject);
        }
    }

    IEnumerator NomalAttackCT()
    {
        Monster = GameObject.FindGameObjectsWithTag("Monster");
        Back = false;
        int r = Random.Range(0, Monster.Length);

        while(true)
        {
            if(Monster[r] != null)
            {
                home = false;

                rig.MovePosition(Vector3.Lerp(transform.position,Monster[r].transform.position,20 * Time.deltaTime));

                if (Vector3.Distance(transform.position, Monster[r].transform.position) <= 0.5f)
                {
                    ani.SetTrigger("attack");
                }

                yield return new WaitForSeconds(0.3f);
                Back = true;
                break;
            }
            yield return null;
        }
    }

    private void Update()
    {
        if(Back == true)
        {
            rig.MovePosition(Vector3.Lerp(transform.position, OriPos, 20 * Time.deltaTime));

            if(Vector3.Distance(transform.position, OriPos)<= 0.5f)
            {
                transform.position = OriPos;
                home = true;
            }
        }
    }
}
