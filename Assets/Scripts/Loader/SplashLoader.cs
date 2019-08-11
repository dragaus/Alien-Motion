using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SplashLoader : MonoBehaviour
{
    GameObject barricaGamesSplash;
    GameObject ccdSplash;
    // Start is called before the first frame update
    void Start()
    {
        barricaGamesSplash = transform.Find("Barrrica Splash").gameObject;
        ccdSplash = transform.Find("CCD Splash").gameObject;
        barricaGamesSplash.SetActive(false);
        ccdSplash.SetActive(false);

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

        barricaGamesSplash.SetActive(false);
        ccdSplash.gameObject.SetActive(true);

        var video = ccdSplash.GetComponent<VideoPlayer>();
        video.Play();
        while (video.isPlaying)
        {
            yield return null;
        }

        Loader.SceneToLoad = "MainMenu";
        SceneManager.LoadScene("Loader");
    }
}
