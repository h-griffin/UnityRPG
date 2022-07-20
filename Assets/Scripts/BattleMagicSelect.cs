using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// access to text
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour
{

    public string spellName;
    public int spellCost;
    public Text nameText;
    public Text costText;


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
        //check if player has enough magic -MP
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
            BattleManager.instance.magicMenu.SetActive(false);
            BattleManager.instance.OpenTargetMenu(spellName);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;
        }
        else
        {
            // not enough mana
        }


    }
}
