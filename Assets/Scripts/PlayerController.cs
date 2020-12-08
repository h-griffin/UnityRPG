using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // MOVEMENT
    public Rigidbody2D theRB;
    public float moveSpeed;

    public Animator myAnim; 
    
    public static PlayerController instance;    // only one player

    public string areaTransitionName; // exit just used

    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    public bool canMove = true; // menue/transition

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)    // only first player
        {
            instance = this;    // 'this' current player
        }
        else 
        {
            if(instance != this)
            {
                Destroy(gameObject); // no two players
            }
        }

        DontDestroyOnLoad(gameObject); // new scene, same session
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1 )
        {
            if (canMove)
            {
                myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
        }

        // keep player in map bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
            Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
            transform.position.z);
    }

    public void SetBounds(Vector3 botLeft, Vector3 topRight) // call from camera with tilemap limits
    {
        // receive map limits from camera + keep player away from actual boarder
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }
}
