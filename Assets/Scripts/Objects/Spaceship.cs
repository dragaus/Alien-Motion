using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    Animator animator;

    Transform posA;
    Transform posB;

    const float maxYValue = 4.5f;
    float speed;
    float rotationSpeed;

    Vector3 posToGo;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        posA = GameObject.Find("Pos A").transform;
        posB = GameObject.Find("Pos B").transform;

        if (Random.Range(0, 2) == 1)
        {
            posToGo = new Vector2(posA.position.x, Random.Range(-maxYValue, maxYValue));

            transform.position = new Vector2(posB.position.x, Random.Range(-maxYValue, maxYValue));
            float randomSize = Random.Range(0.3f, 1f);
            transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        }
        else
        {
            posToGo = new Vector2(posB.position.x, Random.Range(-maxYValue, maxYValue));

            transform.position = new Vector2(posA.position.x, Random.Range(-maxYValue, maxYValue));
            float randomSize = Random.Range(0.3f, 1f);
            transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        }


        speed = Random.Range(1f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(posToGo, transform.position) > 0.2f)
        {
            transform.Translate((posToGo - transform.position).normalized * speed * Time.deltaTime);
        }
        else
        {
            NewSpaceship();
        }
    }

    void NewSpaceship()
    {
        var planet = Instantiate(this.gameObject);
        planet.name = "Spaceship";
        Destroy(this.gameObject);
    }
}
