using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public AudioSource seSource;
public void OnClick3Player()
{
    seSource.Play();
    GameManager.Instance.playerCount = 3;
    SceneManager.LoadScene("MaineGame");
}

public void OnClick4Player()
{
    seSource.Play();
    GameManager.Instance.playerCount = 4;
    SceneManager.LoadScene("MaineGame");
}
}