using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [HideInInspector]public int HpMax;
    [HideInInspector] public int DamagePlayer;
    [HideInInspector] public int Level;
    //
    [HideInInspector] public int DamageEnemy;
    [HideInInspector] public int HpMaxEnemy;
    [HideInInspector] public bool Ketlieu;
    [HideInInspector] public bool MultipleHp;
    [HideInInspector] public int DefaultHpMax; // itembuff
    [HideInInspector] public bool RecoverHpWhenMove;
    [HideInInspector] public bool Bloodsucking;
    [HideInInspector] public bool CounterRate;
    [HideInInspector] public int Critical2Turns;

    public void SavePlayer()
    {
        HpMax = PlayerController.HpMax;
        DamagePlayer = PlayerController.DamageDefault;
        Level = PlayerController.Level;
        Ketlieu = ItemBuff.KetLieu;
        MultipleHp = ItemBuff.MultipleHp;
        DefaultHpMax = ItemBuff.DefaultHpMax;
        RecoverHpWhenMove = ItemBuff.RecoverHpWhenMove;
        Bloodsucking = ItemBuff.Bloodsucking;
        CounterRate = ItemBuff.CounterRate;
        //
        DamageEnemy = Enemy.damageEnemy;
        HpMaxEnemy = Enemy.HpEnemyMax;
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if(data != null)
        {
            PlayerController.HpMax = data.HpMax;
            PlayerController.DamageDefault = data.DamagePlayer;
            PlayerController.Level = data.Level;
            PlayerController.load = true;
            ItemBuff.KetLieu = data.KetLieu;
            ItemBuff.MultipleHp= data.MultipleHp;
            ItemBuff.DefaultHpMax = data.DefaultHpMax;
            ItemBuff.RecoverHpWhenMove= data.RecoverHpWhenMove;
            ItemBuff.Bloodsucking= data.Bloodsucking;
            ItemBuff.CounterRate= data.CounterRate;
            ItemBuff.Critical2Turns = 2;
            //
            Enemy.HpEnemyMax = data.HpMaxEnemy;
            Enemy.damageEnemy = data.DamageEnemy;
            Enemy.load = true;
        }
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
