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
        SceneManager.LoadScene("Demo_01");
    }
}
