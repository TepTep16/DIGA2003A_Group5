using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    [SerializeField]
    //Used to track where the player is relative to the enemy
    private Transform player;

    private float movementX;
    [SerializeField]
    float moveForce;
    [SerializeField]
    float agroRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer < agroRange)
        {
            chasePlayer();
        }
    }

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void chasePlayer()
    {
        //When enemy is to the left of the player and moves towards the right
        if (transform.position.x < player.position.x)
        {
            myBody.linearVelocity = new Vector2(moveForce, 0f);
        }
        /*Vice versa of the code above; don't forget to include negative sign
            to switch direction of movement */

        if (transform.position.x > player.position.x)
        {
            myBody.linearVelocity = new Vector2(-moveForce, 0f);
        }

        if(transform.position.y < player.position.y)
        {
            myBody.linearVelocity = new Vector2(0f, moveForce);
        }
        if(transform.position.y > player.position.y)
        {
            myBody.linearVelocity = new Vector2(0f, -moveForce);
        }
    }
}
