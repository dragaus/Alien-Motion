using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    FirstMenu firstMenu;
    PlayersMenu playersMenu;
    CredistMenu credistMenu;
    ColorMenu colorMenu;

    AudioSource clickManager;

    public MainMenuText mainMenuText;
    const string textDirection = "Menu/MainMenu";

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetString(Keys.languageKey, "en");
        mainMenuText = JsonUtility.FromJson<MainMenuText>(TextAssetLoader.GetCorrectTextAsset(textDirection).text);

        var panel = GameObject.Find("Canvas").transform;
        firstMenu = new FirstMenu(panel.Find("First Menu"));
        playersMenu = new PlayersMenu(panel.Find("Players Menu"));
        credistMenu = new CredistMenu(panel.Find("Credits Menu"));
        colorMenu = new ColorMenu(panel.Find("Color Menu"));

        clickManager = GameObject.Find("Click Manager").GetComponent<AudioSource>();

        playersMenu.howManyPlayerText.text = mainMenuText.numberOfPlayerText;
        for (int i = 0; i < playersMenu.playersButtons.Length; i++)
        {
            playersMenu.playersButtons[i].GetComponentInChildren<Text>().text = $"{i + 2}";
        }

        firstMenu.playButton.onClick.AddListener(()=> ButtonFunction(ShowPlayerMenu));
        firstMenu.infoButton.onClick.AddListener(() => ButtonFunction(ShowCreditMenu));
        for (int i = 0; i < playersMenu.playersButtons.Length; i++)
        {
            int x = i + 2;
            playersMenu.playersButtons[i].onClick.AddListener(() => ButtonFunction(()=> ShowColorMenu(x)));
        }
        playersMenu.returnButton.onClick.AddListener(()=> ButtonFunction(ShowFirstMenu));
        credistMenu.returnButton.onClick.AddListener(() => ButtonFunction(ShowFirstMenu));
        credistMenu.malenyButton.onClick.AddListener(() => Application.OpenURL("https://www.linkedin.com/in/elena-hernández/"));
        credistMenu.pacoButton.onClick.AddListener(() => Application.OpenURL("https://www.linkedin.com/in/francisco-rovira/"));
        for (int i = 0; i < colorMenu.colorButton.Length; i++)
        {
            int x = i;
            colorMenu.colorButton[i].onClick.AddListener(() => ButtonFunction(() => SetColor(x)));
        }
        colorMenu.returnButton.onClick.AddListener(() => ButtonFunction(ReturnColorMenu));

        credistMenu.madeText.text = mainMenuText.creditsBody;
        credistMenu.andText.text = mainMenuText.creditsSecondBody;
    }

    void HideAllMenus()
    {
        firstMenu.gameObject.SetActive(false);
        playersMenu.gameObject.SetActive(false);
        credistMenu.gameObject.SetActive(false);
        colorMenu.gameObject.SetActive(false);
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

    void ShowCreditMenu()
    {
        HideAllMenus();
        credistMenu.gameObject.SetActive(true);
    }

    void SetColor(int colorValue)
    {
        GamePreferences.setPlayers++;
        GamePreferences.playersColors.Add(colorValue);
        if (GamePreferences.setPlayers == GamePreferences.numberOfPleyers)
        {
            Loader.SceneToLoad = "Game";
            SceneManager.LoadScene("Loader");
        }
        else
        {
            ShowColorMenu();
        }
    }

    void ShowColorMenu()
    {
        for (int i = 0; i < colorMenu.colorButton.Length; i++)
        {
            if (GamePreferences.playersColors.Contains(i))
            {
                colorMenu.colorButton[i].gameObject.SetActive(false);
            }
            else
            {
                colorMenu.colorButton[i].gameObject.SetActive(true);
            }
        }
    }

    void ShowColorMenu(int numberOfPlayers)
    {
        GamePreferences.numberOfPleyers = numberOfPlayers;
        GamePreferences.setPlayers = 0;
        GamePreferences.playersColors.Clear();
        HideAllMenus();
        colorMenu.gameObject.SetActive(true);
        ShowColorMenu();
    }

    void ReturnColorMenu()
    {
        GamePreferences.setPlayers = 0;
        ShowPlayerMenu();
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

class CredistMenu
{
    public GameObject gameObject;
    public Button returnButton;
    public Button malenyButton;
    public Button pacoButton;
    public Text madeText;
    public Text andText;

    public CredistMenu(Transform panel)
    {
        gameObject = panel.gameObject;
        var creditPanel = panel.Find("Credits Panel");
        returnButton = creditPanel.Find("Return Button").GetComponent<Button>();
        malenyButton = creditPanel.Find("Maleny Button").GetComponent<Button>();
        pacoButton = creditPanel.Find("Paco Button").GetComponent<Button>();
        madeText = creditPanel.Find("Made Text").GetComponent<Text>();
        andText = creditPanel.Find("And Text").GetComponent<Text>();

        gameObject.SetActive(false);
    }
}

class ColorMenu
{
    public GameObject gameObject;
    public Text colorText;
    public Button[] colorButton;
    public Button returnButton;

    public ColorMenu(Transform panel)
    {
        gameObject = panel.gameObject;

        colorButton = new Button[6];
        for (int i = 0; i < colorButton.Length; i++)
        {
            colorButton[i] = panel.Find($"Color ({i})").GetComponent<Button>();
        }
        returnButton = panel.Find("Return Button").GetComponent<Button>();

        gameObject.SetActive(false);
    }

}

public class MainMenuText
{
    public string creditsBody;
    public string creditsSecondBody;
    public string howBody;
    public string numberOfPlayerText;
    public string selectColorText;
}
