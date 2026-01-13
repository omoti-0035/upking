using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void OnClick3Player()
    {
        GameManager.Instance.playerCount = 3;
        SceneManager.LoadScene("MaineGame");
    }

    public void OnClick4Player()
    {
        GameManager.Instance.playerCount = 4;
        SceneManager.LoadScene("MaineGame");
    }
}