using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dot : MonoBehaviour
{

    public int column;
    public int row;
    public int previouscolumn;
    public int previousrow;
    public int targetx;
    public int targety;
    public bool ismatched = false;
    private GameObject otherdot;
    private board board1;
    private Vector2 firsttouchposition;
    private Vector2 finaltouchposition;
    private Vector2 tempposition;
    public float swipeangle = 0;
    // Start is called before the first frame update
    void Start()
    {
        board1 = FindObjectOfType<board>();
        targetx = (int)transform.position.x;
        targety = (int)transform.position.y;
        row = targety;
        column = targetx;
        previousrow = row;
        previouscolumn = column;

    }

    // Update is called once per frame
    void Update()
    {
        findmathes();
        if (ismatched)
        {
            SpriteRenderer mysprite = GetComponent<SpriteRenderer>();
            mysprite.color = new Color(1f, 1f, 1f, .2f);
        }
        targetx = column;
        targety = row;
        if(Mathf.Abs(targetx - transform.position.x) > .1)
        {
            tempposition = new Vector2(targetx, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempposition, .4f);

        }
        else {
            tempposition = new Vector2(targetx, transform.position.y);
            transform.position = tempposition;
            board1.alldots[column, row] = this.gameObject;
        }
        if (Mathf.Abs(targety - transform.position.y) > .1)
        {
            tempposition = new Vector2(transform.position.x, targety);
            transform.position = Vector2.Lerp(transform.position, tempposition, .4f);

        }
        else
        {
            tempposition = new Vector2(transform.position.x, targety);
            transform.position = tempposition;
            board1.alldots[column, row] = this.gameObject;
        }

    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if(otherdot != null)
        {
            if (!ismatched && !otherdot.GetComponent<dot>().ismatched)
            {
                otherdot.GetComponent<dot>().row = row;
                otherdot.GetComponent<dot>().column = column;
                row = previousrow;
                column = previouscolumn;
            }
            otherdot = null;

        }
    }

    private void OnMouseDown()
    {
        firsttouchposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firsttouchposition);

    }

    private void OnMouseUp()
    {
        finaltouchposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        calculateangle();
    }

    void calculateangle()
    {
        swipeangle = Mathf.Atan2(finaltouchposition.y - firsttouchposition.y, finaltouchposition.x - firsttouchposition.x) * 180 / Mathf.PI;
        //Debug.Log(swipeangle);
        movepiecies();
    }

    void movepiecies()
    {
        if (swipeangle > -45 && swipeangle <= 45 && column < board1.width-1)
        {//right 
            otherdot = board1.alldots[column + 1, row];
            otherdot.GetComponent<dot>().column -= 1;
            column += 1;
        }
        else if (swipeangle > 45 && swipeangle <= 135 && row < board1.height-1)
        {//up
            otherdot = board1.alldots[column, row+1];
            otherdot.GetComponent<dot>().row -= 1;
            row += 1;
        }
        else if ((swipeangle > 135 || swipeangle <= -135) && column >0)
        {//left
            otherdot = board1.alldots[column - 1, row];
            otherdot.GetComponent<dot>().column += 1;
            column -= 1;
        }
        else if (swipeangle < -45 && swipeangle >= -135 && row>0)
        {//down
            otherdot = board1.alldots[column, row-1];
            otherdot.GetComponent<dot>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }

    void findmathes()
    {
        if (column > 0 && column < board1.width - 1)
        {
            GameObject leftdot1 = board1.alldots[column - 1, row];
            GameObject rightdot1 = board1.alldots[column + 1, row];
            if(leftdot1.tag == this.gameObject.tag && rightdot1.tag == this.gameObject.tag)
            {
                leftdot1.GetComponent<dot>().ismatched = true;
                rightdot1.GetComponent<dot>().ismatched = true;
                ismatched = true;
            }
        }
        if (row > 0 && row < board1.height - 1)
        {
            GameObject updot1 = board1.alldots[column, row + 1];
            GameObject downdot1 = board1.alldots[column, row - 1];
            if (updot1.tag == this.gameObject.tag && downdot1.tag == this.gameObject.tag)
            {
                updot1.GetComponent<dot>().ismatched = true;
                downdot1.GetComponent<dot>().ismatched = true;
                ismatched = true;
            }
        }
    }
}