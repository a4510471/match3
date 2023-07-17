using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuscript : MonoBehaviour
{
    public void playgame()
    {
        SceneManager.LoadScene(1);
    }

    public void  quitgame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void records()
    {
        Debug.Log("Poka ne rabotaet :(");
    }

    public void goback()
    {
        SceneManager.LoadScene(0);
    }
}
