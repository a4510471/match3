using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gamestate
{
    wait,
    move
}

public class board : MonoBehaviour
{
    private findmatches findmatches1;
    public gamestate currentstate = gamestate.move;
    public int width;
    public int height;
    public GameObject tileprefab;
    public GameObject[] dots;
    private backgroundtile[,] alltiles;
    public GameObject[,] alldots;
    private scoremanager scoremanager1;
    public int basepiecevalue = 5;
    private int streakvalue = 1;

    // Start is called before the first frame update
    void Start()
    {
        scoremanager1 = FindObjectOfType<scoremanager>();
        findmatches1 = FindObjectOfType<findmatches>();
        alltiles = new backgroundtile[width, height];
        alldots = new GameObject[width, height];
        setup();
    }

    private void setup()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempposition = new Vector2 (i, j);
                GameObject backgrounftile = Instantiate(tileprefab, tempposition ,Quaternion.identity) as GameObject;
                backgrounftile.transform.parent = this.transform;
                backgrounftile.name = "( " + i + ", " + j + " )";
                int dottouse = Random.Range(0, dots.Length);
                int maxiterations = 0;

                while(matchesat(i,j,dots[dottouse])&&maxiterations<100)
                {
                    dottouse = Random.Range(0, dots.Length);
                    maxiterations++;
                }
                maxiterations = 0;

                GameObject dot = Instantiate(dots[dottouse], tempposition, Quaternion.identity);
                dot.transform.parent = this.transform;
                dot.name = "( " + i + ", " + j + " )";
                alldots[i, j] = dot;
            }
        }
    }

    private bool matchesat(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (alldots[column - 1, row].tag == piece.tag && alldots[column - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (alldots[column, row-1].tag == piece.tag && alldots[column, row-2].tag == piece.tag)
            {
                return true;
            }
        }else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if(alldots[column, row-1].tag == piece.tag && alldots[column,row-2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (alldots[column-1, row].tag == piece.tag && alldots[column -2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void destoymatchesat(int column, int row)
    {
        if (alldots[column, row].GetComponent<dot>().ismatched)
        {
            findmatches1.currentmatches.Remove(alldots[column, row]);
            Destroy(alldots[column, row]);
            scoremanager1.increasescore(basepiecevalue*streakvalue);
            alldots[column, row] = null;
        }
    }

    public void destroymatches()
    {
        for(int i = 0; i<width;i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (alldots[i, j] != null)
                {
                    destoymatchesat(i, j);

                }
            }
        }
        StartCoroutine(decreaserowco());
    }

    private IEnumerator decreaserowco()
    {
        int nullcount = 0;
        for (int i = 0; i< width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (alldots[i, j] == null)
                {
                    nullcount++;
                }else if (nullcount > 0)
                {
                    alldots[i, j].GetComponent<dot>().row -= nullcount;
                    alldots[i, j] = null;
                }
            }
            nullcount = 0;

        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(fillboardco());
    }

    private void refillboard()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (alldots[i, j] == null)
                {
                    Vector2 tempposition = new Vector2(i, j);
                    int dottouse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dottouse], tempposition, Quaternion.identity);
                    alldots[i, j] = piece;

                }
            }
        }
    }
    private bool matchesonboard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (alldots[i, j] != null)
                {
                    if (alldots[i, j].GetComponent<dot>().ismatched)
                    {
                        return true;
                    }
                }
            }
        }
                
        return false;

    }

    private IEnumerator fillboardco()
    {
        refillboard();
        yield return new WaitForSeconds(.5f);

        while (matchesonboard())
        {
            streakvalue += 1;
            yield return new WaitForSeconds(.5f);
            destroymatches();
        }
        yield return new WaitForSeconds(.5f);

        if (isdeadlocked())
        {
            Debug.Log("deadlocked");
        }
        currentstate = gamestate.move;
        streakvalue = 1;
    }

    private void switchpieces(int column, int row, Vector2 direction)
    {
        GameObject holder = alldots[column +(int)direction.x, row + (int)direction.y] as GameObject;
        alldots[column + (int)direction.x, row + (int)direction.y] = alldots[column, row];
        alldots[column, row] = holder;

    }
    private bool checkformatches()
    {
        for(int i =0;i<width;i++)
           {
            for (int j = 0; j < height; j++)
            {
                if (alldots[i, j] != null)
                {
                    if (i < width - 2)
                    {
                        if (alldots[i + 1, j] != null && alldots[i + 2, j] != null)
                        {
                            if (alldots[i + 1, j].tag == alldots[i, j].tag && alldots[i + 2, j].tag == alldots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                    if (j < height - 2)
                    {
                        if (alldots[i, j + 1] != null && alldots[i, j + 2] != null)
                        {
                            if (alldots[i, j + 1].tag == alldots[i, j].tag && alldots[i, j + 2].tag == alldots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool switchandcheck(int column,int row, Vector2 direction)
    {
        switchpieces(column, row, direction);
        if(checkformatches())
        {
            switchpieces(column, row, direction);
            return true;
        }
        switchpieces(column, row, direction);
        return false;
    }

    private bool isdeadlocked()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j=0;j< height;j++)
             {
                if(alldots[i,j]!=null)
                {
                    if (i < width - 1)
                    {
                        if(switchandcheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }
                    if(j< height - 1)
                    {
                        if (switchandcheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }

            }
        }
        return true;
    }
    /*private void shuffleboard()
    {
        List<GameObject> newboard = new List<GameObject>();
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (alldots[i, j] != null)
                {
                    newboard.Add(alldots[i, j]);
                }
            }
        }
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(!blankSpaces[])
            }
        }

    }*/
}
