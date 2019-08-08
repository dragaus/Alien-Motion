using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const string prefabObjectPath = "Prefabs/Objects/";
    const string alienObjectPath = "Prefabs/Alien/Alien";

    GameObject barricaModel;
    GameObject panqueModel;

    List<GameObject> barricasSetInPlaced = new List<GameObject>();
    GameObject panqueSetInPlace;

    Transform spawnManager;
    Transform alienManager;

    List<Alien> aliens = new List<Alien>();

    int aliensAlive;

    public float timeOfPanqueMode = 5f;

    // Start is called before the first frame update
    void Start()
    {
        barricaModel = Resources.Load<GameObject>($"{prefabObjectPath}barrica");
        panqueModel = Resources.Load<GameObject>($"{prefabObjectPath}panque");

        spawnManager = GameObject.Find("Spawn Manager").transform;
        alienManager = GameObject.Find("Alien Manager").transform;

        SetAliens();
        SetPanqueLocation();
    }

    // Update is called once per frame
    void Update()
    {

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

        for (int i = 0; i < alienManager.childCount; i++)
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
                panqueSetInPlace = Instantiate(panqueModel, spawnManager.GetChild(i));
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
                aliens[i].SetResult();
            }
        }
    }
}
