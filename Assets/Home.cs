using UnityEngine;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public AudioSource seSource;
    public void HomeButton()
    {
        {
            seSource.Play();
            SceneManager.LoadScene("Stert");
        }
    }
}