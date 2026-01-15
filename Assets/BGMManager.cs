using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource bgm;

    void Start()
    {
        bgm.Play();
    }
}