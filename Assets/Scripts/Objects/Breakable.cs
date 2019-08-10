using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    Animator animator;
    BoxCollider2D[] colliders;

    void Start()
    {
        animator = GetComponent<Animator>();
        colliders = GetComponents<BoxCollider2D>();
    }

    /// <summary>
    /// This set the correct routine to destroy an object to show the inside
    /// </summary>
    public void Break()
    {
        animator.Play("Open");
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
        StartCoroutine(HideObject());
    }

    IEnumerator HideObject()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
