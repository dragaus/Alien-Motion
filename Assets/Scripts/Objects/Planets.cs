using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planets : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    Transform posA;
    Transform posB;

    const float maxYValue = 4.5f;
    float speed;
    float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/Planets/planet{Random.Range(0, 3)}");

        posA = GameObject.Find("Pos A").transform;
        posB = GameObject.Find("Pos B").transform;

        posA.position = new Vector2(posA.position.x, Random.Range(-maxYValue, maxYValue));

        transform.position = new Vector2(posB.position.x, Random.Range(-maxYValue, maxYValue));
        float randomSize = Random.Range(0.3f, 1f);
        transform.localScale = new Vector3(randomSize, randomSize, randomSize);

        spriteRenderer.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));

        speed = Random.Range(1f, 3f);
        rotationSpeed = Random.Range(5f, 10f);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > posA.position.x)
        {
            transform.Translate((posA.position - transform.position).normalized * speed * Time.deltaTime);
            spriteRenderer.transform.eulerAngles = new Vector3(0, 0, spriteRenderer.transform.eulerAngles.z + (rotationSpeed * Time.deltaTime));
        }
        else
        {
            NewPlanet();
        }
    }

    /// <summary>
    /// Create a planet when the planet became invisible
    /// </summary>
    void NewPlanet()
    {
        var planet = Instantiate(this.gameObject);
        planet.name = "planet";
        Destroy(this.gameObject);
    }
}
