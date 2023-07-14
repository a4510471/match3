using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scoremanager : MonoBehaviour
{
    public TMP_Text scoretext;
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoretext.text = score.ToString();
    }
    public void increasescore(int amounttoincrease)
    {
        score += amounttoincrease;

    }
}
