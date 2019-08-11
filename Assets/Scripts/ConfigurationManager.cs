using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfigurationManager : MonoBehaviour
{
    Text configurationText;
    Button returnButton;

    GameObject selectionMenu;
    Button languageButton;
    Button controlButton;

    GameObject languageMenu;
    Button spanishButton;
    Button englishButton;

    GameObject inputMenu;
    List<Slider> inputMethod = new List<Slider>();
    List<Text> players = new List<Text>();

    AudioSource cliker;
    ConfigText textJson;

    // Start is called before the first frame update
    void Start()
    {
        cliker = GetComponent<AudioSource>();
        var panel = GameObject.Find("Canvas").transform;
        configurationText = panel.Find("Configuration Text").GetComponent<Text>();
        returnButton = panel.Find("Return Button").GetComponent<Button>();

        selectionMenu = panel.Find("Select Menu").gameObject;

        languageButton = selectionMenu.transform.Find("Language Button").GetComponent<Button>();
        controlButton = selectionMenu.transform.Find("Control Button").GetComponent<Button>();
        languageButton.onClick.AddListener(() => ButtonFunction(ShowLanguageMenu));
        controlButton.onClick.AddListener(() => ButtonFunction(ShowControlMenu));

        languageMenu = panel.Find("Language Menu").gameObject;
        spanishButton = languageMenu.transform.Find("Spanish Button").GetComponent<Button>();
        englishButton = languageMenu.transform.Find("English Button").GetComponent<Button>();
        spanishButton.onClick.AddListener(() => ButtonFunction(() => ChangeLanguage("es")));
        englishButton.onClick.AddListener(() => ButtonFunction(() => ChangeLanguage("en")));

        inputMenu = panel.Find("Control Menu").gameObject;
        for (int i = 0; i < Keys.playerControllers.Length; i++)
        {
            inputMethod.Add(inputMenu.transform.Find($"Slider ({i})").GetComponent<Slider>());
            players.Add(inputMenu.transform.Find($"Player ({i})").GetComponent<Text>());
        }

        for (int i = 0; i < inputMethod.Count; i++)
        {
            var x = i;
            inputMethod[x].onValueChanged.AddListener(delegate { ButtonFunction(() => PlayerPrefs.SetInt(Keys.playerControllers[x], (int)inputMethod[x].value)); });
            inputMethod[x].value = PlayerPrefs.GetInt(Keys.playerControllers[x]);
        }

        SetTexts();
        ShowSelectionMenu();
    }

    void ButtonFunction(UnityEngine.Events.UnityAction action)
    {
        cliker.Play();
        action();
    }

    void ChangeLanguage(string newLanguage)
    {
        PlayerPrefs.SetString(Keys.languageKey, newLanguage);
        SetTexts();
        configurationText.text = textJson.languageTitle;
    }

    void SetTexts()
    {
        textJson = JsonUtility.FromJson<ConfigText>(TextAssetLoader.GetCorrectTextAsset("Menu/Configuration").text);
        languageButton.GetComponentInChildren<Text>().text = textJson.languageButton;
        controlButton.GetComponentInChildren<Text>().text = textJson.controlsButton;
        spanishButton.GetComponentInChildren<Text>().text = textJson.spansih;
        englishButton.GetComponentInChildren<Text>().text = textJson.english;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].text = $"{textJson.player} {i + 1}";
        }
    }

    void HideAllCanvas()
    {
        selectionMenu.SetActive(false);
        languageMenu.SetActive(false);
        inputMenu.gameObject.SetActive(false);
    }

    void ShowSelectionMenu()
    {
        HideAllCanvas();
        configurationText.text = textJson.configTitle;
        selectionMenu.gameObject.SetActive(true);

        returnButton.onClick.RemoveAllListeners();
        returnButton.onClick.AddListener(() => ButtonFunction(() => SceneManager.LoadScene("MainMenu")));
    }

    void ShowLanguageMenu()
    {
        HideAllCanvas();

        configurationText.text = textJson.languageTitle;
        languageMenu.gameObject.SetActive(true);

        returnButton.onClick.RemoveAllListeners();
        returnButton.onClick.AddListener(() => ButtonFunction(ShowSelectionMenu));
    }

    void ShowControlMenu()
    {
        HideAllCanvas();

        inputMenu.gameObject.SetActive(true);

        returnButton.onClick.RemoveAllListeners();
        returnButton.onClick.AddListener(() => ButtonFunction(ShowSelectionMenu));
    }

}

class ConfigText
{
    public string configTitle;
    public string languageButton;
    public string controlsButton;
    public string languageTitle;
    public string spansih;
    public string english;
    public string controlTitle;
    public string player;
}
