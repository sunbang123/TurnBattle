using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;
    public Dictionary<string, GameObject> D_Player = new Dictionary<string, GameObject>();

    public List<GameObject> L_Monster = new List<GameObject>();

    //플레이어 3개
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;

    //플레이어 3개
    public GameObject Monster1;
    public GameObject Monster2;
    public GameObject Monster3;

    //상태창
    public GameObject[] Status;
    Text[] swordmanTxt;
    Text[] priestTxt;
    Text[] witchTxt;

    // 전체 턴
    public Slider Turn;
    public Text Turntxt;
    public float Turntime = 10;
    CoolTime ct;

    public bool PlayTurn = true;
    public bool MonsterTurn = false;
    public bool CurrTurn = false;

    private void Awake()
    {
        ins = this;
        ct = new CoolTime();
    }
    // Start is called before the first frame update
    void Start()
    {
        //딕션너리쪽에서 데이터를 넣어서 관리한다.
        D_Player.Add("검사", Player1);
        D_Player.Add("신관", Player2);
        D_Player.Add("마법사", Player3);

        L_Monster.Add(Monster1);
        L_Monster.Add(Monster2);
        L_Monster.Add(Monster3);

        //상태창
        Status = GameObject.FindGameObjectsWithTag("Status");

        swordmanTxt = Status[0].GetComponentsInChildren<Text>();
        priestTxt = Status[1].GetComponentsInChildren<Text>();
        witchTxt = Status[2].GetComponentsInChildren<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        Turn.value = ct.Timer(Turntime);

        if(Turn.value >= 0)
        {
            PlayTurn = !PlayTurn;
            CurrTurn = !PlayTurn;

            if(PlayTurn)
            {
                Turntxt.text = "Player Turn";
                MonsterTurn = false;
            }
            else
            {
                MonsterTurn = true;
                Turntxt.text = "Monster Turn";
                StartCoroutine("MonsterAttack");
            }
        }

        //PlayTurn = !PlayTurn;
        //CurrTurn = PlayTurn;
        //CurrTurn = !CurrTurn;

        Debug.Log("CurrTurn  " + CurrTurn);

        //상태표시창
        StatusShow(); // StatusShow(sm);

    }

    //상태표시함수
    void StatusShow() // String a
    {
            //딕셔너리에서 검사를 찾아야한다.
            if (D_Player.ContainsKey("검사"))
            {
                //소드맨오브젝트
                Player P = D_Player["검사"].GetComponent<Player>();

                if (P != null)
                {
                    //플레이어 가져옴
                    swordmanTxt[0].text = P.Pdata.Job;
                    swordmanTxt[1].text = "레벨             " + P.Pdata.Level;
                    swordmanTxt[2].text = "경험치         " + P.Pdata.Exp;
                    swordmanTxt[3].text = "HP           " + P.Pdata.Hp + "/" + P.Pdata.MaxHp;
                    swordmanTxt[4].text = "MP           " + P.Pdata.Mp + "/" + P.Pdata.MaxMp;
                }
            }

            //딕셔너리에서 신관을 찾아야한다.
            if (D_Player.ContainsKey("신관"))
            {
                //소드맨오브젝트
                Player P2 = D_Player["신관"].GetComponent<Player>();

                if (P2 != null)
                {
                    //플레이어 가져옴
                    priestTxt[0].text = P2.Pdata.Job;
                    priestTxt[1].text = "레벨             " + P2.Pdata.Level;
                    priestTxt[2].text = "경험치         " + P2.Pdata.Exp;
                    priestTxt[3].text = "HP           " + P2.Pdata.Hp + "/" + P2.Pdata.MaxHp;
                    priestTxt[4].text = "MP           " + P2.Pdata.Mp + "/" + P2.Pdata.MaxMp;
                }
            }

            //딕셔너리에서 마법사를 찾아야한다.
            if (D_Player.ContainsKey("마법사"))
            {
                //소드맨오브젝트
                Player P3 = D_Player["마법사"].GetComponent<Player>();

                if (P3 != null)
                {
                    //플레이어 가져옴
                    witchTxt[0].text = P3.Pdata.Job;
                    witchTxt[1].text = "레벨             " + P3.Pdata.Level;
                    witchTxt[2].text = "경험치         " + P3.Pdata.Exp;
                    witchTxt[3].text = "HP           " + P3.Pdata.Hp + "/" + P3.Pdata.MaxHp;
                    witchTxt[4].text = "MP           " + P3.Pdata.Mp + "/" + P3.Pdata.MaxMp;
                }
            }
    }

    IEnumerator MonsterAttack()
    {
        int i = 0;
        while(MonsterTurn)
        {
            if (L_Monster.Count != 0)
            {
                L_Monster[(i++) % L_Monster.Count].
                    GetComponent<Monster>().NomalAttack();
            }
            yield return new WaitForSeconds(2f);
        }
    }
}