using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findmatches : MonoBehaviour
{
    private board board1;
    public List<GameObject> currentmatches = new List <GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        board1 = FindAnyObjectByType<board>();
    }

    public void findallmatches()
    {
        StartCoroutine(findallmatchesco());
    }

    private IEnumerator findallmatchesco()
    {
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i < board1.width; i++)
        {
            for(int j=0; j < board1.height; j++)
            {
                GameObject currentdot = board1.alldots[i, j];
                if (currentdot != null)
                {
                    if (i > 0 && i < board1.width - 1)
                    {
                        GameObject leftdot = board1.alldots[i - 1, j];
                        GameObject rightdot = board1.alldots[i + 1, j];
                        if (leftdot != null && rightdot != null)
                        {
                            if(leftdot.tag==currentdot.tag && rightdot.tag == currentdot.tag)
                            {
                                if(!currentmatches.Contains(leftdot))
                                {
                                    currentmatches.Add(leftdot);
                                }
                                leftdot.GetComponent<dot>().ismatched = true;
                                if (!currentmatches.Contains(rightdot))
                                {
                                    currentmatches.Add(rightdot);
                                }
                                rightdot.GetComponent<dot>().ismatched = true;
                                if (!currentmatches.Contains(currentdot))
                                {
                                    currentmatches.Add(currentdot);
                                }
                                currentdot.GetComponent<dot>().ismatched = true;
                            }
                        }

                    }
                    if (j > 0 && j < board1.height - 1)
                    {
                        GameObject updot = board1.alldots[i, j+1];
                        GameObject downdot = board1.alldots[i, j-1];
                        if (updot != null && downdot != null)
                        {
                            if (updot.tag == currentdot.tag && downdot.tag == currentdot.tag)
                            {
                                if (!currentmatches.Contains(updot))
                                {
                                    currentmatches.Add(updot);
                                }
                                updot.GetComponent<dot>().ismatched = true;
                                if (!currentmatches.Contains(downdot))
                                {
                                    currentmatches.Add(downdot);
                                }
                                downdot.GetComponent<dot>().ismatched = true;
                                if (!currentmatches.Contains(currentdot))
                                {
                                    currentmatches.Add(currentdot);
                                }
                                currentdot.GetComponent<dot>().ismatched = true;
                            }
                        }

                    }
                }
            }
        }
    }

}
