using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//access to text
using UnityEngine.UI;

public class BattleReward : MonoBehaviour
{

    public static BattleReward instance;

    public Text xpText, itemText;
    public GameObject rewardScreen;

    public string[] rewardItems;
    public int xpEarned;




    // Start is called before the first frame update
    void Start()
    {
        instance = this;

       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardScreen(54, new string[] { "Iron Sword", "Iron Armor" });
        }
    }

    public void OpenRewardScreen(int xp, string[] rewards)
    {
        xpEarned = xp;
        rewardItems = rewards;

        xpText.text = "Everyone Earned " + xpEarned + " xp!";
        itemText.text = "";

        for (int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += rewards[i] + "\n";
        }

        rewardScreen.SetActive(true);
    }

    public void CloseRewardScreen()
    {
        // apply xp  
        for(int i = 0; i < GameManager.instance.playerStats.Length; i++)
        {
            if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
            {
                GameManager.instance.playerStats[i].AddExp(xpEarned);
            }
        }

        // give items
        for(int i = 0; i <rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardItems[i]);
        }


        rewardScreen.SetActive(false);
        // while reward screen up dont open menues or move
        GameManager.instance.battleActive = false;

    }
}
