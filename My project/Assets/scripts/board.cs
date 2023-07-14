using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tileprefab;
    public GameObject[] dots;
    private backgroundtile[,] alltiles;
    public GameObject[,] alldots;
    // Start is called before the first frame update
    void Start()
    {
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
}
