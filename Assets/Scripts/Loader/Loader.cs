using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public static string SceneToLoad = "Game";

    AsyncOperation newScene;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadNewScene());
    }

    IEnumerator LoadNewScene()
    {
        newScene = SceneManager.LoadSceneAsync(SceneToLoad);
        newScene.allowSceneActivation = false;
        yield return new WaitForSeconds(2.0f);

        while (newScene.progress < .9f)
        {
            yield return null;
        }

        newScene.allowSceneActivation = true;
    }
}
