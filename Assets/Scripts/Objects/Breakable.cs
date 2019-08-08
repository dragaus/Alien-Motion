using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    /// <summary>
    /// This set the correct routine to destroy an object to show the inside
    /// </summary>
    public void Break()
    {
        gameObject.SetActive(false);
    }
}
