using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Alien alien;
    // Start is called before the first frame update
    void Start()
    {
        alien = FindObjectOfType<Alien>();
    }

    // Update is called once per frame
    void Update()
    {
        alien.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
