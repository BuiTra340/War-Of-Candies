using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    Board board;
    public List<GameObject> currentMatches = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }
    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }
    IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for(int i=0;i< board.width;i++)
        {
            for(int j=0;j< board.height;j++)
            {
                GameObject currentCandy = board.allCandies[i,j];
                if(currentCandy !=null)
                {
                    if(i>0 && i< board.width-1)
                    {
                        GameObject leftCandy = board.allCandies[i - 1, j];
                        GameObject rightCandy = board.allCandies[i + 1, j];
                        if(leftCandy !=null && rightCandy !=null)
                        {
                            if(leftCandy.tag == currentCandy.tag && rightCandy.tag == currentCandy.tag)
                            {
                                if(!currentMatches.Contains(leftCandy))
                                {
                                    currentMatches.Add(leftCandy);
                                }
                                leftCandy.GetComponent<Candy>().isMatches = true;
                                if (!currentMatches.Contains(rightCandy))
                                {
                                    currentMatches.Add(rightCandy);
                                }
                                rightCandy.GetComponent<Candy>().isMatches = true;
                                if (!currentMatches.Contains(currentCandy))
                                {
                                    currentMatches.Add(currentCandy);
                                }
                                currentCandy.GetComponent<Candy>().isMatches = true;
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject downCandy = board.allCandies[i , j - 1];
                        GameObject upCandy = board.allCandies[i, j + 1];
                        if (downCandy != null && upCandy != null)
                        {
                            if (downCandy.tag == currentCandy.tag && upCandy.tag == currentCandy.tag)
                            {
                                if (!currentMatches.Contains(downCandy))
                                {
                                    currentMatches.Add(downCandy);
                                }
                                downCandy.GetComponent<Candy>().isMatches = true;
                                if (!currentMatches.Contains(upCandy))
                                {
                                    currentMatches.Add(upCandy);
                                }
                                upCandy.GetComponent<Candy>().isMatches = true;
                                if (!currentMatches.Contains(currentCandy))
                                {
                                    currentMatches.Add(currentCandy);
                                }
                                currentCandy.GetComponent<Candy>().isMatches = true;
                            }
                        }
                    }
                }
            }
        }
    }
    
}
