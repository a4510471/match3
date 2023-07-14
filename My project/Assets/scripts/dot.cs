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

    private findmatches findmatches1;
    private GameObject otherdot;
    private board board1;
    private Vector2 firsttouchposition;
    private Vector2 finaltouchposition;
    private Vector2 tempposition;
    public float swipeangle = 0;
    public float swiperesist = 1f;
    // Start is called before the first frame update
    void Start()
    {
        board1 = FindObjectOfType<board>();
        findmatches1 = FindObjectOfType<findmatches>();
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
        //findmathes();
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
            transform.position = Vector2.Lerp(transform.position, tempposition, .6f);
            if (board1.alldots[column, row] != this.gameObject)
            {
                board1.alldots[column, row] = this.gameObject;
            }
            findmatches1.findallmatches();
        }
        else {
            tempposition = new Vector2(targetx, transform.position.y);
            transform.position = tempposition;
           
        }
        if (Mathf.Abs(targety - transform.position.y) > .1)
        {
            tempposition = new Vector2(transform.position.x, targety);
            transform.position = Vector2.Lerp(transform.position, tempposition, .6f);
            if (board1.alldots[column, row] != this.gameObject)
            {
                board1.alldots[column, row] = this.gameObject;
            }
            findmatches1.findallmatches();
        }
        else
        {
            tempposition = new Vector2(transform.position.x, targety);
            transform.position = tempposition;
       
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
                yield return new WaitForSeconds(.5f);
                board1.currentstate = gamestate.move;
            }
            else
            {
                board1.destroymatches();
                
            }
            otherdot = null;
        }
        
    }

    private void OnMouseDown()
    {
        if (board1.currentstate == gamestate.move)
        {
            firsttouchposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
            //Debug.Log(firsttouchposition);

    }

    private void OnMouseUp()
    {
        if (board1.currentstate == gamestate.move)
        {
            finaltouchposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } 
        calculateangle();
    }

    void calculateangle()
    {
        if (Mathf.Abs(finaltouchposition.y - firsttouchposition.y) > swiperesist || Mathf.Abs(finaltouchposition.x - firsttouchposition.x) > swiperesist)
        { 
            swipeangle = Mathf.Atan2(finaltouchposition.y - firsttouchposition.y, finaltouchposition.x - firsttouchposition.x) * 180 / Mathf.PI;
        //Debug.Log(swipeangle);
            movepiecies();
            board1.currentstate = gamestate.wait;
        }
        else{
            board1.currentstate = gamestate.move;
        }
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
            if (leftdot1 != null && rightdot1 != null)
            {
                if (leftdot1.tag == this.gameObject.tag && rightdot1.tag == this.gameObject.tag)
                {
                    leftdot1.GetComponent<dot>().ismatched = true;
                    rightdot1.GetComponent<dot>().ismatched = true;
                    ismatched = true;
                }
            }
        }
        if (row > 0 && row < board1.height - 1)
        {
            GameObject updot1 = board1.alldots[column, row + 1];
            GameObject downdot1 = board1.alldots[column, row - 1];
            if (updot1 != null && downdot1 != null)
            {
                if (updot1.tag == this.gameObject.tag && downdot1.tag == this.gameObject.tag)
                {
                    updot1.GetComponent<dot>().ismatched = true;
                    downdot1.GetComponent<dot>().ismatched = true;
                    ismatched = true;
                }
            }
        }
    }
}