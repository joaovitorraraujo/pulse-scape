using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene(1);
    }

    public void Sair()
    {
        Application.Quit();
    }
}
