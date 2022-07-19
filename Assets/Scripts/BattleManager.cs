using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    public static BattleManager instance;

    private bool battleActive;

    public GameObject battleScene;

    // positions
    public Transform[] playerPositions;
    public Transform[] enemyPositions;


    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

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
            BattleStart( new string[] { "Eyeball", "Spider" });
        }
    }

    public void BattleStart(string[] enemiesToSpawn)
    {
        if (!battleActive)
        {
            battleActive = true;
            
            GameManager.instance.battleActive = true;

            battleScene.SetActive(true);


        }
    }
}
