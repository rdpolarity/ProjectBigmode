using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ArenaOne(){
        SceneManager.LoadScene("ArenaOne");
    }

    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame(){
        Application.Quit();
    }
}
