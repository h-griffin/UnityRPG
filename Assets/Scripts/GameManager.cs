using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public CharStats[] playerStats;

    // two public bools   v             
    public bool gameMenuOpen, fadeingBetweenAreas; // dialogActive,;

    public string[] itemsHeld;
    public int[] numberofItems;
    public Item[] refereceItems;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen  || fadeingBetweenAreas) //|| dialogActive
        {
            PlayerController.instance.canMove = false;
        }
        else
        {
            PlayerController.instance.canMove = true;
        }
    }

    public Item GetItemDetails(string ItemToGrab)
    {
        for (int i = 0; i < refereceItems.Length; i++)
        {
            if(refereceItems[i].itemName == ItemToGrab)
            {
                return refereceItems[i];
            }
        }


        return null;
    }
}
