using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// where items are held
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

        if (Input.GetKeyDown(KeyCode.J)) //test
        {
            AddItem("IronArmor");  // add
            AddItem("Blah");        // error

            RemoveItem("HealthPotion");
            RemoveItem("bleep");
        }
    }

    public Item GetItemDetails(string ItemToGrab)
    {
        ItemToGrab = ItemToGrab.Trim();
        for (int i = 0; i < refereceItems.Length; i++)
        {
            if(refereceItems[i].itemName == ItemToGrab)
            {
                return refereceItems[i];
            }
        }


        return null;
    }

    public void SortItems()
    {
        // remove blank item spots
        bool itemsAfterSpace = true;

        while (itemsAfterSpace)
        {
            itemsAfterSpace = false;
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1]; // empty grab next one
                    itemsHeld[i + 1] = "";

                    numberofItems[i] = numberofItems[i + 1];
                    numberofItems[i + 1] = 0;

                    if(itemsHeld[i] != "")
                    {
                        itemsAfterSpace = true;
                    }
                }
            }

        }
    }

    public void AddItem(string itemToAdd)
    {
        // find empty or matching item space to add (after game sorts)
        int newItemPosition = 0;
        bool foundSpace = false;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemsHeld.Length; // break
                foundSpace = true;
            }
        }

        if (foundSpace)
        {
            // check if item is valid in game (reference items)
            bool itemExists = false;
            for(int i = 0; i < refereceItems.Length; i++)
            {
                if (refereceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;
                    i = refereceItems.Length; // break
                }
            }
            if (itemExists)
            {
                // new item held in inventory
                itemsHeld[newItemPosition] = itemToAdd;
                numberofItems[newItemPosition]++;
            } else
            {
                // no ref item
                Debug.LogError(itemToAdd + " Does not exist !!!!!!");
            }
        }
        // new item shown in invesntory
        GameMenu.instance.showItems();
    }
    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if(itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;
                i = itemsHeld.Length; // break
            }
        }
        if (foundItem)
        {
            numberofItems[itemPosition]--;
            if(numberofItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
            }
            GameMenu.instance.showItems();
        }
        else
        {
            Debug.LogError("couldnt find " + itemToRemove);
        }
    }
}
