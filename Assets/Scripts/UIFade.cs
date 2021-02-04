using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // <<<<< 

public class UIFade : MonoBehaviour
{
    public static UIFade instance;

    public Image fadeScreen;
    public float fadeSpeed;

    private bool shouldFadeToBlack;
    private bool shouldFaceFromBlack;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeToBlack)
        {
            GameManager.instance.fadeingBetweenAreas = true;

            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime)); //time between updates > keeps fadespeed same on all computer types

            if(fadeScreen.color.a == 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        if (shouldFaceFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                shouldFadeToBlack = false;
                GameManager.instance.fadeingBetweenAreas = false;
            }
        }

    }

    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFaceFromBlack = false;
    }

    public void FadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFaceFromBlack = true; 
    }
}
