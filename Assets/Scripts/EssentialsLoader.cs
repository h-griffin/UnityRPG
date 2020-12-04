using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoader : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        if (UIFade.instance == null)
        {
            Instantiate(UIScreen);
        }

        if(PlayerController.instance == null)
        {
            Instantiate(Player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
