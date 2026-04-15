using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    //Temporary stores the position of the camera
    private Vector3 tempPos;
    //Start position of where the camera follows the player
    [SerializeField]
    private float minX;
    [SerializeField]
    private float minY;
    //End poisition of where the cammera follows the player
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float maxY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Occurs once all calculations in the regular update function are complete.
    private void LateUpdate()
    {
        tempPos = transform.position;
        tempPos.x = player.position.x;
        tempPos.y = player.position.y;

        if (tempPos.x < minX)
        {
            tempPos.x = minX;
        }
        if (tempPos.x > maxX)
        {
            tempPos.x = maxX;
        }
        if(tempPos.y < minY)
        {
            tempPos.y = minY;
        }
        if (tempPos.y > maxY)
        {
            tempPos.y = maxY;
        }

        transform.position = tempPos;
    }
}
