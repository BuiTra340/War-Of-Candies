using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static int HpEnemy;
    public static int HpEnemyMax;
    public static int damageEnemy;
    Animator anim;
    PlayerController playerController;
    public bool canAttack;
    private float Distance;
    public bool moveBack;
    public bool isBleeding;
    //public Transform OriginalPosition;
    Board board;
    private float timeEnemyCanAttack = 1f;
    public Image Hpbar;
    public static bool load;
    ItemBuff item;
    // Start is called before the first frame update
    void Start()
    {
        if(!load)
        {
            damageEnemy = 2;
            HpEnemyMax = 20;
        }
        HpEnemy = HpEnemyMax;
        moveBack = true;
        anim = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        board = FindObjectOfType<Board>();
        item = FindObjectOfType<ItemBuff>();
    }

    // Update is called once per frame
    void Update()
    {
        Hpbar.fillAmount = (float)HpEnemy / HpEnemyMax;
        Distance = Vector2.Distance(playerController.transform.position, transform.position);
        if (canAttack)
        {
            moveBack = false;
            if (Distance <= 1.8f && playerController.moveBack)
            {
                anim.SetBool("Run", false);
                anim.SetTrigger("Attack");
                canAttack = false;

            }
            else
            {
                anim.SetBool("Run", true);
                transform.position = Vector2.MoveTowards(transform.position, playerController.transform.position, 5 * Time.deltaTime);
            }
        }
        if (moveBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(6,8.6f), 13 * Time.deltaTime);
        }

        if (board.state == GameState.move && playerController.TurnAttack == 0 && HpEnemy > 0 && playerController.Distance >= 5.5f)
        {
            if (timeEnemyCanAttack > 0)
            {
                timeEnemyCanAttack -= Time.deltaTime;
            }
            else
            {
                board.state = GameState.wait;
                CheckEnemyAttack();
            }
        }
    }
    public void takeDamage(int damage)
    {
        HpEnemy -= damage;
        if (HpEnemy > 0)
        {
            anim.SetTrigger("Hurt");
        }
        else
        {
            HpEnemy = 0;
            anim.SetTrigger("Death");
            if(isBleeding)
            {
                DestroyBleed des = FindObjectOfType<DestroyBleed>();
                des.DesBleedEffect();
            }
            playerController.Win = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerController>().takeDamage(damageEnemy - (int)playerController.ShieldStat / 2);
            StartCoroutine(WaitMoveBack());
            item.CounterRateEnemy();
        }
    }
    public void CheckEnemyAttack()
    {
        StartCoroutine(CheckEnemyAttackCo());
    }
    IEnumerator CheckEnemyAttackCo()
    {
        yield return new WaitForSeconds(.5f);
        if (playerController.TurnAttack == 0)
        {
            canAttack = true;
        }
    }
    IEnumerator WaitMoveBack()
    {
        yield return new WaitForSeconds(.5f);
        if(PlayerController.CurrentHp > 0) moveBack = true;
        yield return new WaitForSeconds(.5f);
        board.state = GameState.move;
        playerController.TurnAttack = 2;
        timeEnemyCanAttack = 1f;
    }
}
