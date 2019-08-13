using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const string prefabObjectPath = "Prefabs/Objects/";
    const string alienObjectPath = "Prefabs/Alien/Alien";

    GameMenu gameMenu;

    MusicManager musicManager;
    AudioSource audioSource;

    GameObject barricaModel;
    GameObject panqueModel;

    List<GameObject> barricasSetInPlaced = new List<GameObject>();
    GameObject panqueSetInPlace;

    Transform spawnManager;
    Transform alienManager;
    Transform borderManager;

    List<Alien> aliens = new List<Alien>();

    public int numberOfPlayersWithKeyboard = 0;
    public int numberOfPlayersWithJoystick = 0;

    int aliensAlive;
    string winText;

    public float timeOfPanqueMode = 5f;
    public List<Color> possibleColors;

    public AudioClip hurtSound;
    public AudioClip gotSound;
    public AudioClip yeahSound;

    // Start is called before the first frame update
    void Start()
    {
        gameMenu = new GameMenu(GameObject.Find("Game Menu").transform);
        winText = TextAssetLoader.GetCorrectTextAsset("Game/Game").text;
        gameMenu.otherGameButton.onClick.AddListener(() => ButtonFunction(() => ChangeScene("Game")));
        gameMenu.homeButton.onClick.AddListener(() => ButtonFunction(() => ChangeScene("MainMenu")));
        gameMenu.pauseButton.onClick.AddListener(() => ButtonFunction(ShowPauseMenu));
        gameMenu.resumeButton.onClick.AddListener(() => ButtonFunction(HidePauseMenu));
        gameMenu.goHomeButton.onClick.AddListener(() => ButtonFunction(GoHome));

        barricaModel = Resources.Load<GameObject>($"{prefabObjectPath}barrica");
        panqueModel = Resources.Load<GameObject>($"{prefabObjectPath}panque");

        spawnManager = GameObject.Find("Spawn Manager").transform;
        alienManager = GameObject.Find("Alien Manager").transform;
        borderManager = GameObject.Find("Border Manager").transform;

        musicManager = FindObjectOfType<MusicManager>();
        musicManager.PlayNewClip(musicManager.gameMusic);

        audioSource = GetComponent<AudioSource>();

        SetColors();
        SetAliens();
        SetPanqueLocation();
    }

    void ChangeScene(string sceneName)
    {
        Loader.SceneToLoad = sceneName;
        SceneManager.LoadScene("Loader");
    }

    void ButtonFunction(UnityEngine.Events.UnityAction action)
    {
        audioSource.Play();
        action();
    }

    public void SetColors()
    {
        List<int> colorsToSet = new List<int>();
        for (int i = 0; i < Keys.plastilineColors.Length; i++)
        {
            int x = i;
            colorsToSet.Add(x);
        }

        foreach (int c in GamePreferences.playersColors)
        {
            colorsToSet.Remove(c);
        }

        int random = Random.Range(0, colorsToSet.Count);
        random = colorsToSet[random];

        for (int i = 0; i < Keys.plastilineColors.Length; i++)
        {
            if (i != random)
            {
                ColorUtility.TryParseHtmlString($"#{Keys.plastilineColors[i]}", out Color c);
                possibleColors.Add(c);
            }
            else
            {
                SetBorders(i);
            }
        }
    }

    public void SetBorders(int colorNum)
    {
        ColorUtility.TryParseHtmlString($"#{Keys.plastilineColors[colorNum]}", out Color c);
        for (int i = 0; i < borderManager.childCount; i++)
        {
            var renderer = borderManager.GetChild(i).GetComponent<SpriteRenderer>();
            renderer.color = c;
            var border = borderManager.GetChild(i).gameObject.AddComponent<BoxCollider2D>();
            border.size = renderer.size;
        }

        var limitManager = GameObject.Find("LimitManager").transform;
        for (int i = 0; i < limitManager.childCount; i++)
        {
            limitManager.GetChild(i).GetComponent<SpriteRenderer>().color = c;
        }
    }

    /// <summary>
    /// This set all the aliens in their correct place
    /// </summary>
    public void SetAliens()
    {
        List<Transform> spawnPositions = new List<Transform>();
        for (int i = 0; i < alienManager.childCount; i++)
        {
            spawnPositions.Add(alienManager.GetChild(i));
        }

        for (int i = 0; i < GamePreferences.numberOfPlayers; i++)
        {
            int pos = Random.Range(0, spawnPositions.Count);
            var positionToStart = spawnPositions[pos];
            var alien = Instantiate(Resources.Load<GameObject>(alienObjectPath),positionToStart).GetComponent<Alien>();
            int x = i;
            alien.SetAlienInfo(x, this);
            aliens.Add(alien);
            spawnPositions.Remove(positionToStart);
        }

        aliensAlive = aliens.Count;
    }


    /// <summary>
    /// Set the barrels and panque in the correct place
    /// </summary>
    public void SetPanqueLocation()
    {
        int panquePos = Random.Range(0, spawnManager.childCount);

        for (int i = 0; i < spawnManager.childCount; i++)
        {
            var barrelToAdd = Instantiate(barricaModel, spawnManager.GetChild(i));
            barricasSetInPlaced.Add(barrelToAdd);
            if (panquePos == i)
            {
                panqueSetInPlace = Instantiate(panqueModel, barrelToAdd.transform);
                panqueSetInPlace.gameObject.SetActive(false);
            }
        }
        musicManager.audioSource.pitch = 1f;
    }


    /// <summary>
    /// this should be call when the panque mode is over
    /// </summary>
    public void EndPanqueRoutine()
    {
        for (int i = 0; i < barricasSetInPlaced.Count; i++)
        {
            Destroy(barricasSetInPlaced[i]);
        }

        timeOfPanqueMode++;
        barricasSetInPlaced.Clear();
        panqueSetInPlace = null;
        musicManager.audioSource.pitch = 1.30f;
    }

    /// <summary>
    /// this is call when an alien hits another to get track of the aliens
    /// </summary>
    /// <param name="numberOfAlien"></param>
    public void AlienIsDown(int numberOfAlien)
    {
        aliens[numberOfAlien].StopMoving();

        aliensAlive--;

        if (aliensAlive <= 1)
        {
            for (int i = 0; i < aliens.Count; i++)
            {
                if (aliens[i].IsWinner())
                {
                    ShowResults(aliens[i].GetAlienId());
                }
            }
        }
    }

    public void ShowPauseMenu()
    {
        gameMenu.pauseButton.gameObject.SetActive(false);
        gameMenu.pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void HidePauseMenu()
    {
        gameMenu.pauseButton.gameObject.SetActive(true);
        gameMenu.pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void GoHome()
    {
        Time.timeScale = 1;
        Loader.SceneToLoad = "MainMenu";
        SceneManager.LoadScene("Loader");
    }

    public void ShowResults(int winnerNumber)
    {
        musicManager.audioSource.pitch = 1f;
        gameMenu.resultMenu.gameObject.SetActive(true);
        gameMenu.pauseButton.gameObject.SetActive(false);
        gameMenu.resultText.text = string.Format(winText, winnerNumber + 1);
        ColorUtility.TryParseHtmlString($"#{Keys.plastilineColors[GamePreferences.playersColors[winnerNumber]]}", out Color c);
        gameMenu.resultText.color = c;
    }
}

class GameMenu
{
    public GameObject gameObject;

    public Transform resultMenu;
    public Text resultText;
    public Button otherGameButton;
    public Button homeButton;

    public Button pauseButton;

    public Transform pauseMenu;
    public Button resumeButton;
    public Button goHomeButton;

    public GameMenu(Transform panel)
    {
        gameObject = panel.gameObject;

        resultMenu = panel.Find("Result Menu");
        resultText = resultMenu.Find("Tittle Text").GetComponent<Text>();
        otherGameButton = resultMenu.Find("PlayAginButton").GetComponent<Button>();
        homeButton = resultMenu.Find("HomeButton").GetComponent<Button>();

        pauseButton = panel.Find("Pause Button").GetComponent<Button>();

        pauseMenu = panel.Find("Pause Menu");
        var pausePanel = pauseMenu.Find("Pause Panel");
        resumeButton = pausePanel.Find("Resume Button").GetComponent<Button>();
        goHomeButton = pausePanel.Find("Home Button").GetComponent<Button>();

        resultMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
    }
}