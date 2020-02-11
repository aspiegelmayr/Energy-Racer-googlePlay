using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// manages match finding
/// </summary>
public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void findAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    /// <summary>
    /// finds all matches on board
    /// </summary>
    /// <returns></returns>
    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(0.2f);
        for(int i = 0; i < board.width; i++)
        {
            for(int j  = 0; j < board.height; j++)
            {
                GameObject curDot = board.allDots[i, j];
                if(curDot != null)
                {
                    if(i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if(rightDot != null && leftDot != null)
                        {
                            if(rightDot.tag == curDot.tag && leftDot.tag == curDot.tag)
                            {
                                leftDot.GetComponent<Dot>().isMatched = true;
                                curDot.GetComponent<Dot>().isMatched = true;
                                rightDot.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i, j-1];
                        GameObject downDot = board.allDots[i, j+1];
                        if (downDot != null && upDot != null)
                        {
                            if (downDot.tag == curDot.tag && upDot.tag == curDot.tag)
                            {
                                downDot.GetComponent<Dot>().isMatched = true;
                                curDot.GetComponent<Dot>().isMatched = true;
                                upDot.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
