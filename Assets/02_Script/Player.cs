// ===== Player.cs =====
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData Pdata;

    private GameObject[] monsters;
    private Rigidbody2D rig;
    private Vector3 originalPos;
    private Animator ani;
    private bool isMoving = false;
    private bool isAttacking = false;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
        ani = GetComponent<Animator>();
        Pdata.Hp = Pdata.MaxHp;
    }

    void Update()
    {
        // 플레이어 턴일 때만 공격 가능
        if (GameManager.ins.PlayTurn && Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            NormalAttack();
        }

        // 돌아오는 중이면 계속 이동
        if (isMoving && !isAttacking)
        {
            MoveBack();
        }
    }

    public void NormalAttack()
    {
        if (!GameManager.ins.PlayTurn || isMoving || isAttacking)
            return;

        monsters = GameObject.FindGameObjectsWithTag("Monster");
        if (monsters.Length == 0)
            return;

        int randomMonster = Random.Range(0, monsters.Length);
        StartCoroutine(AttackCoroutine(monsters[randomMonster]));
    }

    IEnumerator AttackCoroutine(GameObject targetMonster)
    {
        isMoving = true;

        // 몬스터로 이동
        while (targetMonster != null && Vector3.Distance(transform.position, targetMonster.transform.position) > 0.5f)
        {
            Vector3 moveDir = (targetMonster.transform.position - transform.position).normalized;
            rig.velocity = moveDir * 5f;
            yield return null;
        }

        rig.velocity = Vector3.zero;

        // 공격 애니메이션
        if (targetMonster != null)
        {
            isAttacking = true;
            ani.SetTrigger("attack");
            yield return new WaitForSeconds(0.5f);

            // 데미지 적용
            Monster monster = targetMonster.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(Pdata.Attack);
            }

            isAttacking = false;
        }

        isMoving = true;
    }

    void MoveBack()
    {
        if (Vector3.Distance(transform.position, originalPos) > 0.1f)
        {
            Vector3 moveDir = (originalPos - transform.position).normalized;
            rig.velocity = moveDir * 5f;
        }
        else
        {
            transform.position = originalPos;
            rig.velocity = Vector3.zero;
            isMoving = false;
        }
    }

    public void TakeDamage(int damage)
    {
        Pdata.Hp -= damage;
        ani.SetTrigger("damage");

        if (Pdata.Hp <= 0)
        {
            Pdata.Hp = 0;
            GameManager.ins.D_Player.Remove(Pdata.Job);
            Destroy(gameObject);
        }
    }

    void Sound()
    {
        if (Pdata.Job == "검사")
        {
            SoundManager.instance.PlayAttackSound(8);
        }

        if (Pdata.Job == "신관")
        {
            SoundManager.instance.PlayAttackSound(4);
        }

        if (Pdata.Job == "마법사")
        {
            SoundManager.instance.PlayAttackSound(2);
        }
    }
}