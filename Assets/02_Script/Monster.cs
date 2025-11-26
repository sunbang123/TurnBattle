// ===== Monster.cs (수정된 전체 코드) =====
using System.Collections;
using TMPro;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData Mdata;

    private GameObject[] players;
    private Rigidbody2D rig;
    private Vector3 originalPos;
    private Animator ani;
    private bool isMoving = false;
    private bool isAttacking = false;

    public GameObject DamageCanvas;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
        ani = GetComponent<Animator>();
        Mdata.Hp = Mdata.MaxHp;
    }

    void Update()
    {
        if (isMoving && !isAttacking)
        {
            MoveBack();
        }
    }

    public void NormalAttack()
    {
        if (isMoving || isAttacking)
            return;

        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
            return;

        int randomPlayer = Random.Range(0, players.Length);
        if (players[randomPlayer] != null)
            StartCoroutine(AttackCoroutine(players[randomPlayer]));
    }

    IEnumerator AttackCoroutine(GameObject targetPlayer)
    {
        isMoving = true;

        // 플레이어로 이동
        while (targetPlayer != null && Vector3.Distance(transform.position, targetPlayer.transform.position) > 0.5f)
        {
            Vector3 moveDir = (targetPlayer.transform.position - transform.position).normalized;
            rig.velocity = moveDir * 5f;
            yield return null;
        }

        rig.velocity = Vector3.zero;

        // 공격 애니메이션
        if (targetPlayer != null)
        {
            isAttacking = true;
            ani.SetTrigger("attack");
            yield return new WaitForSeconds(0.5f);

            // 데미지 적용
            Player player = targetPlayer.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(Mdata.Attack);
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
        Mdata.Hp -= damage;
        ani.SetTrigger("damage");

        // [수정된 부분]
        // 1. 몬스터의 머리 위에 띄우기 위해 Y축 오프셋 추가
        Vector3 spawnPosition = transform.position;

        GameObject go = Instantiate(DamageCanvas, spawnPosition, Quaternion.identity);

        // 3. 부모를 몬스터(this.transform)로 설정
        //    (Instantiate(prefab, parent) 대신 Instantiate(prefab) 후 SetParent(parent, false)를 사용하는 것이 좋습니다.)
        //    **두 번째 인수에 false를 넣으면 월드 좌표를 유지합니다.**
        go.transform.SetParent(transform, false);

        TextMeshProUGUI TMPdamage = go.GetComponentInChildren<TextMeshProUGUI>();
        TMPdamage.text = damage.ToString();

        if (Mdata.Hp <= 0)
        {
            Mdata.Hp = 0;
            GameManager.ins.RemoveMonster(gameObject);
            Destroy(gameObject);
        }
    }
}