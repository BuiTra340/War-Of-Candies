using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    move,
    wait
}
public class Board : MonoBehaviour
{
    public GameState state = GameState.move;
    public int width;
    public int height;
    public int Offset;
    public GameObject BackroundPrefabs;
    //private BackgroundTitle[,] allTitle;
    public GameObject[] Candies;
    public GameObject[,] allCandies;
    PlayerController playerController;
    public GameObject BleedEffect;
    Enemy enemy;
    FindMatches findMatches;
    public GameObject BannerLevel;
    public Text textLevel;
    // Start is called before the first frame update
    void Start()
    {
        ShowBannerLv();
        enemy = FindObjectOfType<Enemy>();
        playerController = FindObjectOfType<PlayerController>();
        findMatches = FindObjectOfType<FindMatches>();
        //allTitle = new BackgroundTitle[width, height];
        allCandies = new GameObject[width, height];
        Setup();
    }
    void Setup()
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                Vector2 pos = new Vector2(i, j);
                GameObject Backgroundtile =  Instantiate(BackroundPrefabs,pos, Quaternion.identity) as GameObject;
                Backgroundtile.transform.parent = this.transform;
                Backgroundtile.name = ("" + i + " , " + j);

                int maxIterations = 0;
                int CandiesToUse = Random.Range(0, Candies.Length);
                while (MatchesAt(i,j,Candies[CandiesToUse]) && maxIterations < 100)
                {
                    CandiesToUse= Random.Range(0, Candies.Length);
                    maxIterations++;
                }
                maxIterations = 0;
                pos = new Vector2(i, j + Offset);
                GameObject candy = Instantiate(Candies[CandiesToUse], pos, Quaternion.identity);
                candy.GetComponent<Candy>().column = i;
                candy.GetComponent<Candy>().row = j;
                candy.transform.parent = this.transform;
                candy.name = ("" + i + " , " + j);
                allCandies[i, j] = candy; 
            }
        }
    }
    private bool MatchesAt(int column,int row,GameObject piece)
    {
        //dung de khong co su xuat hien 3 o trong 1 hang hoac 1 cot
        if(column > 1 && row > 1)
        {
            if (allCandies[column -1 ,row].gameObject.tag == piece.gameObject.tag && allCandies[column - 2, row].gameObject.tag == piece.gameObject.tag)
            {
                return true;

            }
            if(allCandies[column,row -1].gameObject.tag == piece.gameObject.tag && allCandies[column, row - 2].gameObject.tag == piece.gameObject.tag)
            {
                return true;
            }
        }else if(column <= 1 || row <=1)
        {
            if(column > 1)
            {
                if (allCandies[column - 1, row].gameObject.tag == piece.gameObject.tag && allCandies[column - 2, row].gameObject.tag == piece.gameObject.tag)
                {
                    return true;

                }
            }
            if(row > 1)
            {
                if (allCandies[column, row - 1].gameObject.tag == piece.gameObject.tag && allCandies[column, row - 2].gameObject.tag == piece.gameObject.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void DestroyCandyAt(int column, int row)
    {
        if (allCandies[column, row].GetComponent<Candy>().isMatches)
        {
            CheckTagCandies(column, row);
            findMatches.currentMatches.Remove(allCandies[column, row]);
            Destroy(allCandies[column, row]);
            allCandies[column, row] = null;
        }
    }
    public void DestroyCandy()
    {
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                if(allCandies[i,j] !=null)
                {
                    DestroyCandyAt(i, j);
                }
            }
        }
        checkTagHp();
        checkTagBleed();
        StartCoroutine(DecreaseRowCo());
    }
    IEnumerator DecreaseRowCo()
    {
        int count = 0;
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {
                if(allCandies[i,j] == null)
                {
                    count++;
                }else if(count > 0)
                {
                    allCandies[i, j].GetComponent<Candy>().row -= count;
                    allCandies[i, j] = null;
                }
            }
            count = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allCandies[i, j] == null)
                {
                    Vector2 tempPos = new Vector2(i, j + Offset);
                    int CandytoUse = Random.Range(0, Candies.Length);
                    GameObject candy = Instantiate(Candies[CandytoUse], tempPos, Quaternion.identity);
                    allCandies[i, j] = candy;
                    candy.GetComponent<Candy>().column = i;
                    candy.GetComponent<Candy>().row = j;
                }
            }
        }
        StartCoroutine(CheckIsNotMatches());
    }
    public bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allCandies[i, j] != null)
                {
                    if (allCandies[i, j].GetComponent<Candy>().isMatches)
                    {
                        return true;
                    }
                }   
            }
        }
        return false;
    }
    IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);
        while (MatchesOnBoard())
        {
            //yield return new WaitForSeconds(.5f);
            DestroyCandy();
        }
        //yield return new WaitForSeconds(.5f);
        //state = GameState.move;
    }
    IEnumerator CheckIsNotMatches()
    {
        yield return new WaitForSeconds(1.8f);
        if (!MatchesOnBoard()) state = GameState.move;
    }
    void CheckTagCandies(int columnTag,int rowTag)
    {
        if (allCandies[columnTag, rowTag].gameObject.tag == "Attack")
        {
            playerController.DamageStat++;
            playerController.canAttack = true;
        }
        if (allCandies[columnTag, rowTag].gameObject.tag == "Health")
        {
            playerController.HpStat++;
        }
        if (allCandies[columnTag, rowTag].gameObject.tag == "Fury")
        {
            playerController.FuryStat++;
            if (playerController.FuryStat >= 10) playerController.FuryStat = 10;
        }
        if (allCandies[columnTag, rowTag].gameObject.tag == "Shield")
        {
            playerController.ShieldStat++;
            if (playerController.ShieldStat >= 8) playerController.ShieldStat = 8;
        }
        if (allCandies[columnTag, rowTag].gameObject.tag == "Bleed")
        {
            enemy.isBleeding = true;
        }
    }
    void checkTagHp()
    {
        PlayerController.CurrentHp += playerController.HpStat;
        if (PlayerController.CurrentHp > PlayerController.HpMax) PlayerController.CurrentHp = PlayerController.HpMax;
        playerController.HpStat = 0;
    }
    void checkTagBleed()
    {
        if (enemy.isBleeding && playerController.TurnBleed == 0)
        {
            playerController.TurnBleed = 2;
            Vector2 offset = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 0.5f);
            GameObject effect = Instantiate(BleedEffect,offset, Quaternion.identity);
            effect.transform.parent = enemy.transform;
        }
    }
    void ShowBannerLv()
    {
        textLevel.text = "Level " + PlayerController.Level;
        BannerLevel.SetActive(true);
        StartCoroutine(DesBannerLV());
    }
    IEnumerator DesBannerLV()
    {
        yield return new WaitForSeconds(1.4f);
        BannerLevel.SetActive(false);
    }
}
