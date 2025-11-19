using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Player Data", menuName = "Data/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
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
