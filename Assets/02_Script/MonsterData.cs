using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Monster Data", menuName = "Data/MonsterData", order = 0)]
public class MonsterData : ScriptableObject
{
    public string Pname;
    public string Job;
    public int Hp;
    public int MaxHp;
    public int Attack;
    public int Level;
    public int Mp;
    public int MaxMp;
    public int Exp;
}
