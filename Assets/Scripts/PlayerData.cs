using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int HpMax;
    public int DamagePlayer;
    public int Level;
    public bool KetLieu;
    public bool MultipleHp;
    public int DefaultHpMax; // itembuff
    public bool RecoverHpWhenMove;
    public bool Bloodsucking;
    public bool CounterRate;
    //
    public int DamageEnemy;
    public int HpMaxEnemy;

    public PlayerData(Player player)
    {
        HpMax = player.HpMax;
        DamagePlayer = player.DamagePlayer;
        Level = player.Level;
        KetLieu = player.Ketlieu;
        MultipleHp = player.MultipleHp;
        DefaultHpMax = player.DefaultHpMax;
        RecoverHpWhenMove = player.RecoverHpWhenMove;
        Bloodsucking = player.Bloodsucking;
        CounterRate = player.CounterRate;
        //
        DamageEnemy = player.DamageEnemy; 
        HpMaxEnemy= player.HpMaxEnemy;
    }
}
