using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    Animator animator;

    float speed = 2f;

    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This is used to set the direction of the character if its going left set 0 if is going right set 1
    /// </summary>
    public void Walk(int dir)
    {
        spriteRenderer.flipX = dir == 0;
    }

    public void Move(float horizontal, float vertical)
    {
        if (horizontal != 0 || vertical != 0)
        {
            if (!isMoving)
            {
                animator.Play("Walk");
                isMoving = true;
            }
            transform.Translate(new Vector3(horizontal * speed * Time.deltaTime, vertical * speed * Time.deltaTime, 0));
            if (horizontal > 0)
            {
                Walk(1);
            }
            else if (horizontal < 0)
            {
                Walk(0);
            }
        }
        else
        {
            isMoving = false;
            animator.Play("Idle");
        }
    }

}
