using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    GameManager manager;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    Animator animator;

    Breakable breakable;

    const float normalSpeed = 4f;
    const float panqueSpeed = 4.5f;

    float speed;
    float timeOfPanqueMode;

    int playerNumber = 0;

    Color originalColor;

    bool isMoving = false;
    bool isHiting = false;
    bool isAlive = true;
    bool isInPanqueMode = false;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        speed = normalSpeed;
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        //We need to now if its in panque mode
        if (isInPanqueMode)
        {
            //Chaneg the color of the alien to show the panque mode
            spriteRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            timeOfPanqueMode -= Time.deltaTime;
            if (timeOfPanqueMode <= 0)
            {
                ExitPanqueMode();
            }
        }

        if (Input.GetAxis($"Fire{playerNumber}") > 0)
        {
            Hit();
        }

        Move(Input.GetAxis($"Horizontal{playerNumber}"), Input.GetAxis($"Vertical{playerNumber}"));
    }

    public void SetAlienInfo(int numberOfAlien)
    {
        playerNumber = numberOfAlien;
        ColorUtility.TryParseHtmlString($"#{Keys.plastilineColors[GamePreferences.playersColors[numberOfAlien]]}", out Color colorAlien);
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.color = colorAlien;
    }

    public int GetAlienId()
    {
        return playerNumber;
    }

    /// <summary>
    /// This is used to set the direction of the character if its going left set 0 if is going right set 1
    /// </summary>
    public void Walk(int dir)
    {
        spriteRenderer.flipX = dir == 0;
    }

    /// <summary>
    /// This is call to break a barrica
    /// </summary>
    public void Hit()
    {
        if (!isHiting && isAlive)
        {
            animator.Play("Hit");
            isHiting = true;
            isMoving = false;
        }
    }


    /// <summary>
    /// this is call in the animation of hit in the correct time
    /// </summary>
    public void HitObject()
    {
        //We need to chechk if theres a brakable object in case its not the we avoid the error
        if (breakable != null)
        {
            breakable.Break();
            breakable = null;
        }
    }

    /// <summary>
    /// This is set when a player is kill by another
    /// </summary>
    public void StopMoving()
    {
        isAlive = false;
        boxCollider.enabled = false;
        spriteRenderer.sortingOrder = 2;
        animator.Play("Cry");
    }


    /// <summary>
    /// Call at the end of the Game to set results
    /// </summary>
    public void SetResult()
    {
        //We check if the alien is alive and by that the winner
        if (isAlive)
        {
            isAlive = false;
            animator.Play("Dance");
        }
    }

    /// <summary>
    /// Call when the hit animation is done to let the player move normal again
    /// </summary>
    public void StopHit()
    {
        isHiting = false;
    }

    /// <summary>
    /// Used to move the player the horizontal requiers its proper Horizontal axis and vertical need its porper vertical axis
    /// </summary>
    /// <param name="horizontal"></param>
    /// <param name="vertical"></param>
    public void Move(float horizontal, float vertical)
    {
        if (!isHiting && isAlive)
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

    /// <summary>
    /// Its used to destroy a panque and start the panque mode
    /// </summary>
    /// <param name="panqueToEat"></param>
    void EnterPanqueMode(GameObject panqueToEat)
    {
        speed = panqueSpeed;
        isInPanqueMode = true;
        timeOfPanqueMode = manager.timeOfPanqueMode;
        manager.FindPanqueRoutine();
        Destroy(panqueToEat);
    }

    /// <summary>
    /// Used to escape the panque mode when time its up and restore options to set new barricas
    /// </summary>
    void ExitPanqueMode()
    {
        speed = normalSpeed;
        isInPanqueMode = false;
        spriteRenderer.color = originalColor;
        manager.SetPanqueLocation();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Alien>() && isInPanqueMode)
        {
            manager.AlienIsDown(collision.gameObject.GetComponent<Alien>().GetAlienId());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Breakable>())
        {
            breakable = collision.GetComponent<Breakable>();
        }
        if (collision.tag == "Panque")
        {
            EnterPanqueMode(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        breakable = null;
    }

}
