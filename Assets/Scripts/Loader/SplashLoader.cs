using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SplashLoader : MonoBehaviour
{
    GameObject barricaGamesSplash;
    // Start is called before the first frame update
    void Start()
    {
        barricaGamesSplash = transform.Find("Barrrica Splash").gameObject;
        barricaGamesSplash.SetActive(false);

        StartCoroutine(SplashCorutine());
    }

    IEnumerator SplashCorutine()
    {
        barricaGamesSplash.SetActive(true);
        var sound = barricaGamesSplash.GetComponentInChildren<AudioSource>();
        sound.Play();
        while (sound.isPlaying)
        {
            yield return null;
        }
        Loader.SceneToLoad = "MainMenu";
        SceneManager.LoadScene("Loader");
    }
}
