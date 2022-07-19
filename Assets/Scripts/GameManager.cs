using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// where items are held
public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public CharStats[] playerStats;

    // public bools   v             
    public bool gameMenuOpen, fadeingBetweenAreas, shopActive, battleActive; // dialogActive,;

    public string[] itemsHeld;
    public int[] numberofItems;
    public Item[] refereceItems;

    public int currentgold;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
        SortItems(); // so world items stack in inventory
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen  || fadeingBetweenAreas || shopActive || battleActive) //|| dialogActive
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

        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
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

    public void SaveData()
    {
        // player loaction /scene & cordinates
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_X", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_Y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_Z", PlayerController.instance.transform.position.z);

        // save char info /stats
        for (int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy) // is character active
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Active", 1); // true
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Active", 0); // false
            }
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHp", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHp", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMp", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMp", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defense", playerStats[i].defense);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WpnPwr", playerStats[i].wpnPwr);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armrPwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWpn", playerStats[i].equippedWpn);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equippedArmr);

        }

        // items in inventory
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" + i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmmount_" + i, numberofItems[i]);
        }
    }

    public void LoadData()
    {
        // load player cordinates
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_X"), PlayerPrefs.GetFloat("Player_Position_Y"), PlayerPrefs.GetFloat("Player_Position_Z"));

        // char info /stats
        for (int i = 0; i < playerStats.Length; i++)
        {
            if(PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
            playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentExp");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHp");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHp");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMp");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMp");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].defense = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defense");
            playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WpnPwr");
            playerStats[i].armrPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmrPwr");
            playerStats[i].equippedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWpn");
            playerStats[i].equippedArmr = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmr");
        }

        // inventory info
        for(int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberofItems[i] = PlayerPrefs.GetInt("ItemAmmount_" + i);
        }
    }
}
