// ===== GameManager.cs =====
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;
    public Dictionary<string, GameObject> D_Player = new Dictionary<string, GameObject>();
    public List<GameObject> L_Monster = new List<GameObject>();

    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;

    public GameObject Monster1;
    public GameObject Monster2;
    public GameObject Monster3;

    public GameObject[] Status;
    Text[] swordmanTxt;
    Text[] priestTxt;
    Text[] witchTxt;

    public Slider Turn;
    public Text Turntxt;
    public float Turntime = 10f;

    private CoolTime ct;
    public bool PlayTurn = true;
    public bool MonsterTurn = false;
    private Coroutine monsterAttackCoroutine;
    
    public GameObject WinPanel;

    private void Awake()
    {
        if (ins == null)
            ins = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        D_Player.Add("검사", Player1);
        D_Player.Add("신관", Player2);
        D_Player.Add("마법사", Player3);

        L_Monster.Add(Monster1);
        L_Monster.Add(Monster2);
        L_Monster.Add(Monster3);

        Status = GameObject.FindGameObjectsWithTag("Status");
        swordmanTxt = Status[0].GetComponentsInChildren<Text>();
        priestTxt = Status[1].GetComponentsInChildren<Text>();
        witchTxt = Status[2].GetComponentsInChildren<Text>();

        ct = new CoolTime();
        ct.StartTimer(Turntime);
    }

    void Update()
    {
        // 턴 진행도 표시
        Turn.value = ct.GetProgress();

        // 턴 시간이 끝났는지 확인
        if (ct.IsFinished())
        {
            SwitchTurn();
            ct.StartTimer(Turntime);
        }

        StatusShow();
    }

    void SwitchTurn()
    {
        PlayTurn = !PlayTurn;
        MonsterTurn = !MonsterTurn;

        if (PlayTurn)
        {
            Turntxt.text = "Player Turn";
            // 몬스터 공격 코루틴 중지
            if (monsterAttackCoroutine != null)
                StopCoroutine(monsterAttackCoroutine);
        }
        else
        {
            Turntxt.text = "Monster Turn";
            monsterAttackCoroutine = StartCoroutine(MonsterAttackSequence());
        }
    }

    void StatusShow()
    {
        UpdatePlayerStatus("검사", swordmanTxt);
        UpdatePlayerStatus("신관", priestTxt);
        UpdatePlayerStatus("마법사", witchTxt);
    }

    void UpdatePlayerStatus(string jobName, Text[] textArray)
    {
        if (D_Player.ContainsKey(jobName))
        {
            Player p = D_Player[jobName].GetComponent<Player>();
            if (p != null)
            {
                textArray[0].text = p.Pdata.Job;
                textArray[1].text = "레벨        " + p.Pdata.Level;
                textArray[2].text = "경험치      " + p.Pdata.Exp;
                textArray[3].text = "HP         " + p.Pdata.Hp + "/" + p.Pdata.MaxHp;
                textArray[4].text = "MP         " + p.Pdata.Mp + "/" + p.Pdata.MaxMp;
            }
        }
    }

    IEnumerator MonsterAttackSequence()
    {
        int i = 0;
        while (MonsterTurn && L_Monster.Count > 0)
        {
            // 죽은 몬스터 제거
            L_Monster.RemoveAll(m => m == null);

            if (L_Monster.Count == 0)
            {
                WinPanel.SetActive(true);
                MonsterTurn = false;
                PlayTurn = false;
                yield break;
            }

            GameObject monsterObj = L_Monster[i % L_Monster.Count];
            if (monsterObj != null)
            {
                Monster monster = monsterObj.GetComponent<Monster>();
                if (monster != null)
                {
                    monster.NormalAttack();
                }
            }
            i++;
            yield return new WaitForSeconds(2f);
        }
    }

    public void RemoveMonster(GameObject monster)
    {
        if (L_Monster.Contains(monster))
            L_Monster.Remove(monster);
    }
}