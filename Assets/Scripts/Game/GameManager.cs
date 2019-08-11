using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const string prefabObjectPath = "Prefabs/Objects/";
    const string alienObjectPath = "Prefabs/Alien/Alien";

    GameMenu gameMenu;

    MusicManager musicManager;

    GameObject barricaModel;
    GameObject panqueModel;

    List<GameObject> barricasSetInPlaced = new List<GameObject>();
    GameObject panqueSetInPlace;

    Transform spawnManager;
    Transform alienManager;
    Transform borderManager;

    List<Alien> aliens = new List<Alien>();

    int aliensAlive;
    string winText;

    public float timeOfPanqueMode = 5f;
    public List<Color> possibleColors;

    // Start is called before the first frame update
    void Start()
    {
        gameMenu = new GameMenu(GameObject.Find("Canvas").transform);
        winText = TextAssetLoader.GetCorrectTextAsset("Game/Game").text;

        barricaModel = Resources.Load<GameObject>($"{prefabObjectPath}barrica");
        panqueModel = Resources.Load<GameObject>($"{prefabObjectPath}panque");

        spawnManager = GameObject.Find("Spawn Manager").transform;
        alienManager = GameObject.Find("Alien Manager").transform;
        borderManager = GameObject.Find("Border Manager").transform;

        musicManager = FindObjectOfType<MusicManager>();
        musicManager.PlayGameMusic();

        SetColors();
        SetAliens();
        SetPanqueLocation();
    }

    // Update is called once per frame
    void Update()
    {

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
            borderManager.GetChild(i).GetComponent<SpriteRenderer>().color = c;
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
            alien.SetAlienInfo(x);
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
    }


    /// <summary>
    /// this should be call when the panque mode is over
    /// </summary>
    public void FindPanqueRoutine()
    {
        for (int i = 0; i < barricasSetInPlaced.Count; i++)
        {
            Destroy(barricasSetInPlaced[i]);
        }

        barricasSetInPlaced.Clear();
        panqueSetInPlace = null;
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

    public void ShowResults(int winnerNumber)
    {
        gameMenu.gameObject.SetActive(true);
        gameMenu.resultText.text = string.Format(winText, winnerNumber + 1);
        ColorUtility.TryParseHtmlString($"#{Keys.plastilineColors[GamePreferences.playersColors[winnerNumber]]}", out Color c);
        gameMenu.resultText.color = c;
    }
}

class GameMenu
{
    public GameObject gameObject;
    public Text resultText;

    public GameMenu(Transform panel)
    {
        gameObject = panel.gameObject;
        resultText = panel.Find("Tittle Text").GetComponent<Text>();

        gameObject.SetActive(false);
    }
}