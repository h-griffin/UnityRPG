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


    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)    // only first player
        {
            instance = this;    // 'this' current player
        }
        else 
        {
            Destroy(gameObject); // no two players
        }

        DontDestroyOnLoad(gameObject); // new scene, same session
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;

        myAnim.SetFloat("moveX", theRB.velocity.x);
        myAnim.SetFloat("moveY", theRB.velocity.y);

        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1 )
        {
            myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }

    }
}
