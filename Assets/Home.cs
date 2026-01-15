using UnityEngine;
using UnityEngine.SceneManagement;
public class Home : MonoBehaviour
{
        public AudioSource seSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 public void HomeButton()
    {
        seSource.Play();
        SceneManager.LoadScene("Stert");
        Destroy(GameObject.Find("GameDirector"));
    }
}
