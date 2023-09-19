using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuff : MonoBehaviour
{
    public GameObject[] ItemsLeft;
    public GameObject[] ItemsRight;
    public GameObject[] SkillEffects;
    public static bool KetLieu;
    public static bool MultipleHp;
    public static bool RecoverHpWhenMove;
    public static bool Bloodsucking;
    public static bool CounterRate;
    public static int Critical2Turns;
    PlayerController playerController;
    public static int DefaultHpMax;
    AudioManager audio;
    Enemy enemy;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        audio = FindObjectOfType<AudioManager>();
        enemy = FindObjectOfType<Enemy>();
    }
    public void ShowItems()
    {
        int itemLeft = Random.Range(0, ItemsLeft.Length);
        ItemsLeft[itemLeft].SetActive(true);

        int itemRight = Random.Range(0, ItemsRight.Length);
        ItemsRight[itemRight].SetActive(true);
    }

    public void ClickKetlieu()
    {
        KetLieu = true;
        playerController.NextLevel();
    }
    public void ClickMultipleHp()
    {
        MultipleHp = true;
        playerController.NextLevel();
        DefaultHpMax = PlayerController.HpMax;
        PlayerController.HpMax += (int)PlayerController.HpMax * 50 / 100;
    }
    public void ClickRecoverHp()
    {
        RecoverHpWhenMove = true;
        playerController.NextLevel();
    }
    public void ClickBloodsucking()
    {
        Bloodsucking = true;
        playerController.NextLevel();
    }
    public void ClickCounterRate()
    {
        CounterRate = true;
        playerController.NextLevel();
    }
    public void ClickCritical2Turns()
    {
        Critical2Turns = 2;
        playerController.NextLevel();
    }
    public void ResetBuff()
    {
        KetLieu = false;
        RecoverHpWhenMove = false;
        Bloodsucking = false;
        CounterRate = false;
        Critical2Turns = 0;
        if(MultipleHp)
        {
            PlayerController.HpMax = DefaultHpMax;
            MultipleHp = false;
        }
    }
    public void IncreaseHpWhenMove()
    {
        if(RecoverHpWhenMove)
        {
            PlayerController.CurrentHp += (int)PlayerController.HpMax * 10 / 100;
            audio.PlaySFX(audio.RecoverHp);
            Destroy(Instantiate(SkillEffects[1], new Vector3(0, 8.5f, 0), Quaternion.identity),.5f);
            if(PlayerController.CurrentHp >= PlayerController.HpMax)
            {
                PlayerController.CurrentHp = PlayerController.HpMax;
            }
        }
    }
    public void BloodsuckingEnenmy()
    {
        Vector3 vector3 = new Vector3(playerController.transform.position.x, playerController.transform.position.y - .3f, 0);
        if (Bloodsucking)
        {
            PlayerController.CurrentHp += (int)PlayerController.DamageDefault * playerController.DamageStat * 50 / 100;
            Destroy(Instantiate(SkillEffects[2], vector3, Quaternion.identity), .5f);
            if (PlayerController.CurrentHp >= PlayerController.HpMax)
            {
                PlayerController.CurrentHp = PlayerController.HpMax;
            }
        }
    }
    public void CounterRateEnemy()
    {
        if(CounterRate)
        {
            int rate = Random.Range(30, 101);
            if(rate == 100)
            {
                enemy.takeDamage(Enemy.damageEnemy);
            }
        }
    }
}
