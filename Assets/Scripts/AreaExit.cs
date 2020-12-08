using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;   // <<<<<<<

public class AreaExit : MonoBehaviour
{

    public string areaToLoad;

    public string areaTransitionName; // exit just used

    public AreaEntrance theEnterance;

    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;

    // Start is called before the first frame update
    void Start()
    {
        theEnterance.transitionName = areaTransitionName;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.tag == "Player")
        if(other.tag == "Player")
        {
            //SceneManager.LoadScene(areaToLoad);
            shouldLoadAfterFade = true;
            GameManager.instance.fadeingBetweenAreas = true;

            UIFade.instance.FadeToBlack();


            PlayerController.instance.areaTransitionName = areaTransitionName;
        }
    }
}
