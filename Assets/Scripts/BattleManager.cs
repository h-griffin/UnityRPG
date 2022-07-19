using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<BattleChar> activeBattlers = new List<BattleChar>(); // more versatile than array/dont have to loop

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

        }
    }
}
