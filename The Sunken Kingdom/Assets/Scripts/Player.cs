using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D myBody;
    private SpriteRenderer sr;

    //These variables are used to control the player's movement on the x-axis and y-axis
    private float movementX;
    private float moveForceX = 8f;
    private float movementY;
    private float moveForceY = 8f;

    //Used to check where the enemy is relative to the player
    [SerializeField]
    private Transform enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement();
    }

    private void Awake()
    {
        //Used to call the components of the object
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void playerMovement()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        movementY = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(movementX, 0f, 0f) * Time.deltaTime * moveForceX;
        transform.position += new Vector3(0f, movementY, 0f) * Time.deltaTime * moveForceY;
    }

    void playerCombat()
    {

    }
}
