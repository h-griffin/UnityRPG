using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;   // <<<<<<<

public class AreaExit : MonoBehaviour
{

    public string areaToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void onTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.tag == "Player")
        if(other.tag == "Player")
        {
            SceneManager.LoadScene(areaToLoad);
        }
    }
}
