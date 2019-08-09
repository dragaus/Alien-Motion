using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAssetLoader : MonoBehaviour
{
    public static TextAsset GetCorrectTextAsset(string path)
    {
        return Resources.Load<TextAsset>($"Texts/{PlayerPrefs.GetString(Keys.languageKey, "en")}/{path}");
    }
 }
