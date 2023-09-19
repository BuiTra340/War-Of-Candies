using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static int DamageDefault; // can upgrade
    public static int CurrentHp;
    public static int HpMax;
    public int DamageStat;
    public int FuryStat;
    public int HpStat;
    public int ShieldStat;
    public float speed;
    Animator anim;
    public bool canAttack;
    [HideInInspector]public float Distance;
    Enemy enemy;
    public Transform OriginalPosition;
    public bool moveBack;
    public Image Hpbar;
    public Image Furybar;
    public int TurnAttack = 2;
    public int TurnBleed;
    public bool Win;
    Board board;
    public GameObject BannerWin;
    public GameObject BannerLose;
    public GameObject BannerPause;
    AudioManager audio;
    public static bool load;
    public static int Level;
    Player player;
    public GameObject[] EnemyPrefabs;
    ItemBuff item;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy();
        if (!load)
        {
            HpMax = 10;
            DamageDefault = 1;
            Level = 1;
        }
        CurrentHp = HpMax;
        moveBack = true;
        anim = GetComponent<Animator>();
        enemy = FindObjectOfType<Enemy>();
        board = FindObjectOfType<Board>();
        audio = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<Player>();
        item = FindObjectOfType<ItemBuff>();
    }
    // Update is called once per frame
    void Update()
    {
        Hpbar.fillAmount = (float)CurrentHp / HpMax;
        Furybar.fillAmount = (float)FuryStat / 10;
        Distance = Vector2.Distance(transform.position, enemy.transform.position);
        if(canAttack && Enemy.HpEnemy > 0)
        {
            moveBack = false;
            if (Distance <= 1.8f)
            {
                //
                anim.SetBool("Run", false);
                anim.SetTrigger("Attack");
                canAttack = false;
            }
            else
            {
                board.state = GameState.wait;
                anim.SetBool("Run", true);
                transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, speed * Time.deltaTime);
            }
        }
        if (moveBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, OriginalPosition.position, (speed + 8) * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(ItemBuff.KetLieu && Enemy.HpEnemy <= (int)Enemy.HpEnemyMax * 15/100)
            {
                Destroy(Instantiate(item.SkillEffects[0], new Vector2(5.8f, 8.6f), Quaternion.identity),.5f);
                audio.PlaySFX(audio.Ketlieu);
                collision.gameObject.GetComponent<Enemy>().takeDamage(Enemy.HpEnemyMax);
            }
            else
            {
                if(ItemBuff.Critical2Turns > 0)
                {
                    collision.gameObject.GetComponent<Enemy>().takeDamage(DamageDefault * DamageStat * 2);
                    StartCoroutine(DecreaseFuryStat());
                }
                else
                {
                    if (FuryStat < 10)
                    {
                        collision.gameObject.GetComponent<Enemy>().takeDamage(DamageDefault * DamageStat);
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Enemy>().takeDamage(DamageDefault * DamageStat * 2);
                        StartCoroutine(DecreaseFuryStat());
                    }
                }
            }
            item.BloodsuckingEnenmy();
            StartCoroutine(WaitMoveBack());
            if (Win)
            {
                StartCoroutine(ShowBannerWin());
            }
        }
    }
    IEnumerator DecreaseFuryStat()
    {
        yield return new WaitForSeconds(.1f);
        FuryStat--;
        if (FuryStat > 0) StartCoroutine(DecreaseFuryStat());
    }
    IEnumerator WaitMoveBack()
    {
        yield return new WaitForSeconds(.5f);
        moveBack = true;
        DamageStat = 0;
        yield return new WaitForSeconds(1.8f);
        board.state = GameState.move;
    }
    public void takeDamage(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp > 0)
        {
            if(ShieldStat > 0)
            {
                anim.SetTrigger("Shield");
            }
            else anim.SetTrigger("Hurt");
        }
        else
        {
            CurrentHp = 0;
            anim.SetTrigger("Death");
            StartCoroutine(ShowBannerLose());
        }
    }
    IEnumerator ShowBannerWin()
    {
        item.ResetBuff();
        audio.musicSource.volume /=2;
        yield return new WaitForSeconds(2f);
        audio.PlaySFX(audio.Win);
        if (!BannerWin.activeInHierarchy)
        {
            BannerWin.SetActive(true);
            item.ShowItems();
        }
        Time.timeScale = 0;
    }
    IEnumerator ShowBannerLose()
    {
        board.state = GameState.wait;
        audio.musicSource.volume /= 2;
        yield return new WaitForSeconds(2f);
        audio.PlaySFX(audio.Lose);
        Time.timeScale = 0;
        if (!BannerLose.activeInHierarchy)
        {
            BannerLose.SetActive(true);
        }
    }
    public void NextLevel()
    {
        Time.timeScale = 1;
        BannerWin.SetActive(false);
        Win = false;
        Level++;
        Enemy.HpEnemyMax += 5;
        player.SavePlayer();
        player.LoadPlayer();
    }
    public void RetryGame()
    {
        Time.timeScale = 1;
        BannerLose.SetActive(false);
        player.LoadPlayer();
    }
    public void BackToMenuStart()
    {
        Time.timeScale = 1;
        Win = false;
        BannerWin.SetActive(false);
        SceneManager.LoadScene(0);
    }
    public void PauseGame()
    {
        board.state = GameState.wait;
        Time.timeScale = 0;
        if (!BannerPause.activeInHierarchy) BannerPause.SetActive(true);
    }
    public void ResumeGame()
    {
        board.state = GameState.move;
        Time.timeScale = 1;
        BannerPause.SetActive(false);
    }
    public void EnemyBleed()
    {
        if (TurnBleed > 0)
        {
            Enemy.HpEnemy -= 1;
            if(Enemy.HpEnemy <= 0)
            {
                TurnBleed = 0;
                Enemy.HpEnemy = 0;
                enemy.GetComponent<Animator>().SetTrigger("Death");
                DestroyBleed des = FindObjectOfType<DestroyBleed>();
                des.DesBleedEffect();
                enemy.isBleeding = false;
                Win = true;
                StartCoroutine(ShowBannerWin());
            }
            else
            {
                TurnBleed--;
                if (TurnBleed == 0)
                {
                    DestroyBleed des = FindObjectOfType<DestroyBleed>();
                    des.DesBleedEffect();
                    enemy.isBleeding = false;
                }
            }
        }
    }
    private void SpawnEnemy()
    {
        int enemy1 = Random.Range(0, EnemyPrefabs.Length);
        Vector2 pos = new Vector2(6, 8.6f);
        Instantiate(EnemyPrefabs[enemy1], pos, Quaternion.identity);
    }
}
