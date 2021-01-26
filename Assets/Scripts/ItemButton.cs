using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{

    public Image buttonImage;
    public Text ammountText;
    public int buttonValue;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        if(GameManager.instance.itemsHeld[buttonValue] != "")
        {
            // set active item and update info in game menu
            GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
        }
    }
}
