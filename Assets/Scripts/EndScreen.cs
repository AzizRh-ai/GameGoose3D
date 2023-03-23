using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Scene1()
    {
        SceneManager.LoadScene("StartScreen 1");
    }
}
