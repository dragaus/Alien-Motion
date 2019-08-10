using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    FirstMenu firstMenu;
    PlayersMenu playersMenu;
    AudioSource clickManager;

    public MainMenuText mainMenuText;
    const string textDirection = "Menu/MainMenu";
    // Start is called before the first frame update
    void Start()
    {
        mainMenuText = JsonUtility.FromJson<MainMenuText>(TextAssetLoader.GetCorrectTextAsset(textDirection).text);

        var panel = GameObject.Find("Canvas").transform;
        firstMenu = new FirstMenu(panel.Find("First Menu"));
        playersMenu = new PlayersMenu(panel.Find("Players Menu"));

        clickManager = GameObject.Find("Click Manager").GetComponent<AudioSource>();

        playersMenu.howManyPlayerText.text = mainMenuText.numberOfPlayerText;
        for (int i = 0; i < playersMenu.playersButtons.Length; i++)
        {
            playersMenu.playersButtons[i].GetComponentInChildren<Text>().text = $"{i + 2}";
        }

        firstMenu.playButton.onClick.AddListener(()=> ButtonFunction(ShowPlayerMenu));
        playersMenu.returnButton.onClick.AddListener(()=> ButtonFunction(ShowFirstMenu));


    }

    void HideAllMenus()
    {
        firstMenu.gameObject.SetActive(false);
        playersMenu.gameObject.SetActive(false);
    }

    void ShowFirstMenu()
    {
        HideAllMenus();
        firstMenu.gameObject.SetActive(true);
    }

    void ShowPlayerMenu()
    {
        HideAllMenus();
        playersMenu.gameObject.SetActive(true);
    }

    void ButtonFunction(UnityEngine.Events.UnityAction action)
    {
        clickManager.Play();
        action();
    }
}

class FirstMenu
{
    public GameObject gameObject;
    public Button playButton;
    public Button howButton;
    public Button infoButton;

    public FirstMenu(Transform panel)
    {
        gameObject = panel.gameObject;
        playButton = panel.Find("PlayButton").GetComponent<Button>();
        howButton = panel.Find("How Button").GetComponent<Button>();
        infoButton = panel.Find("CreditsButton").GetComponent<Button>();

        gameObject.SetActive(true);
    }
}

class PlayersMenu
{
    public GameObject gameObject;
    public Text howManyPlayerText;
    public Button[] playersButtons;
    public Button returnButton;

    public PlayersMenu(Transform panel)
    {
        gameObject = panel.gameObject;
        howManyPlayerText = panel.Find("HowManyPlayersText").GetComponent<Text>();
        playersButtons = new Button[3];
        for (int i = 0; i < playersButtons.Length; i++)
        {
            playersButtons[i] = panel.Find($"Players ({i})").GetComponent<Button>();
        }

        returnButton = panel.Find("Return Button").GetComponent<Button>();

        gameObject.SetActive(false);
    }
}

public class MainMenuText
{
    public string creditText;
    public string creditsBody;
    public string howBody;
    public string numberOfPlayerText;
    public string selectColorText;
}
