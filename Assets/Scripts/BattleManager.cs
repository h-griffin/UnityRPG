using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// access to text
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    public static BattleManager instance;

    private bool _battleActive;


    public GameObject battleScene;

    // positions
    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    // players / enemies
    public List<BattleChar> activeBattlers = new List<BattleChar>(); // more versatile than array/dont have to loop

    //turns
    public int currentTurn; //cycle through active batlers
    public bool turnWaiting; // waiting for turn to end (input from ayer or enemy move

    public GameObject uiButtonsHolder;

    // damage
    public BattleMove[] movesList;
    public GameObject enemyAttackEffect;
    public DamageNumber theDamageNumber;

    // ui
    public Text[] playerName,playerHP, playerMP;

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    public BattleNotification battleNotice;

    // flee
    public int chanceToFlee = 35;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BattleStart( new string[] { "Eyeball", "Spider", "Skeleton" });
        }

        // turns
        if (_battleActive)
        {
            if (turnWaiting)
            {
                // player or enemy 
                if (activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);
                } else
                {
                    uiButtonsHolder.SetActive(false);

                    // enemy should atttack
                    StartCoroutine(EnemyMoveCo());
                }
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextTurn();
            }
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if (!_battleActive)
        {
            _battleActive = true;
            
            GameManager.instance.battleActive = true;

            // find camera position to show battle scene, then activate and play music
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);

            AudioManager.instance.PlayBGM(0);

            // add players
            for (int i = 0; i < playerPositions.Length; i++)
            {
                // is player active in game manager - i=player
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    // is prefab same name as player - j=prefab
                    for(int j = 0; j < playerPrefabs.Length; j++)
                    {
                        if(playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            // add to world
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            //if you want to move the player around buring battle (new player for access to positions)
                            newPlayer.transform.parent = playerPositions[i];

                            // track chars to add/sub helath and life
                            activeBattlers.Add(newPlayer);

                            // assign stats to activebattlers
                            CharStats thePlayer = GameManager.instance.playerStats[i];
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defence = thePlayer.defense;
                            activeBattlers[i].weaponPower = thePlayer.wpnPwr;
                            activeBattlers[i].armrPower = thePlayer.armrPwr;


                        }
                    }
                }
            }

            // add enemies
            for(int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if(enemiesToSpawn[i] != "")
                {
                    // enemy prefab
                    for(int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if(enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }

            // start turns
            turnWaiting = true;
            currentTurn = Random.Range(0, activeBattlers.Count);

            UpdateUIStats();
        }
    }

    public void NextTurn()
    {
        currentTurn++;
        if(currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;
        UpdateBattle();
        UpdateUIStats();
    }

    public void UpdateBattle()
    {
        // check deaths
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for(int i = 0; i < activeBattlers.Count; i++)
        {
            // is activebattler health below zero
            if(activeBattlers[i].currentHP < 0)
            {
                activeBattlers[i].currentHP = 0;
            }

            // is active battler health zero
            if(activeBattlers[i].currentHP == 0)
            {
                // handle dead battler
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].aliveSprite;
                }else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if(allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                // end battle in vicory 
            }
            else
            {
                // end battle in failure
            }

            battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            _battleActive = false;
        }
        else
        {
            // battle not over
            while(activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if(currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        // ienumerator co routine - happens outside of normal oredr of things in unity
        // start running and set timer - unity continues 

        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    public void EnemyAttack()
    {
        // pick a target
        List<int> players = new List<int>();
        for (int i = 0; i <activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }

        int selectedTarget = players[Random.Range(0, players.Count)];

        // pick an attack
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;

        // find attack and play effect
        for (int i= 0;i < movesList.Length; i++)
        {
            // can battler use attack
            if(movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[selectedTarget].transform.rotation);
        DealDamage(selectedTarget, movePower);
    }
    
    public void DealDamage(int target, int movePower)
    {
        float attackPower = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].weaponPower;
        float defensePower = activeBattlers[target].defence + activeBattlers[target].armrPower;

        float damageCalc = (attackPower / defensePower) * movePower * Random.Range(.9f, 1.1f) ;
        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + " (" + damageToGive + ") damage to " + activeBattlers[target].charName);
        activeBattlers[target].currentHP -= damageToGive;

        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).setDamage(damageToGive);

        UpdateUIStats();
    }

    // call when deal damage - next turn - battle start
    public void UpdateUIStats()
    {
        for(int i = 0; i < playerName.Length; i++)
        {
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];

                    // clear empty stat slot of no player
                    playerName[i].gameObject.SetActive(true);

                    playerName[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp( playerData.currentMP,0 , int.MaxValue) + "/" + playerData.maxMP;
                }
                else
                {
                    playerName[i].gameObject.SetActive(false);
                }

            }
            else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName , int selectedTarget)

    {

        int movePower = 0;

        // find attack and play effect
        for (int i = 0; i < movesList.Length; i++)
        {
            // can battler use attack
            if (movesList[i].moveName == moveName)
            {
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = movesList[i].movePower;
            }
        }

        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[selectedTarget].transform.rotation);
        DealDamage(selectedTarget, movePower);

        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        NextTurn();
    }


    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        // assign buttons to enemies in scene

        // get list of enemies out of the battlers
        List<int> Enemies = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++){
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }

        // should button be active
        for(int i =0; i < targetButtons.Length; i++)
        {
            // disable enemy target button when dead
            if(Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);

                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }

        }

    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);

        for (int i = 0; i < magicButtons.Length; i++)
        {
            if(activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                // name  - from battler moves available
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].spellName;

                // cost  - from moves list
                for (int j = 0; j < movesList.Length; j++)
                {
                    if(movesList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }

            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        // random change to allow flee
        int fleeSuccess = Random.Range(0, 100);
        if(fleeSuccess < chanceToFlee)
        {
            // end the battle
            _battleActive = false;
            battleScene.SetActive(false);

        }
        else
        {
            // end of turn
            NextTurn();
            battleNotice.theText.text = "could not escape! ";
            battleNotice.Activate();
        }
    }
}
