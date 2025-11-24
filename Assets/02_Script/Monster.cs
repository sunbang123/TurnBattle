using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData Mdata;

    GameObject[] Player;
    Rigidbody2D rig;

    public bool Back = false;
    public Vector3 OriPos;
    Animator ani;

    public int HP;
    public int MaxHP;

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        OriPos = transform.position;
        ani = GetComponent<Animator>();

        HP = Mdata.Hp;
        MaxHP = Mdata.MaxHp;
    }

    public void NomalAttack()
    {
        StartCoroutine("NomalAttackCT");
    }

    IEnumerator NomalAttackCT()
    {
        Player = GameObject.FindGameObjectsWithTag("Player");
        Back = false;
        int r = Random.Range(0, Player.Length);

        while(true)
        {
            if(Player[r] != null)
            {
                rig.MovePosition(Vector3.Lerp(transform.position, Player[r].transform.position,20 * Time.deltaTime));

                if (Vector3.Distance(transform.position, Player[r].transform.position) <= 0.5f)
                {
                    ani.SetTrigger("attack");
                    Player[r].GetComponent<Player>();
                    Damage(Mdata.Attack);
                    yield return new WaitForSeconds(0.3f);
                    Back = true;
                    break;
                }
            }
            yield return null;
        }
    }

    public void Damage(int Attack)
    {
        Mdata.Hp -= Attack;
        ani.SetTrigger("damage");

        if (Mdata.Hp <= 0)
        {
            GameManager.ins.D_Player.Remove(Mdata.Job);
            Destroy(gameObject);
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
            }
        }
    }
}
