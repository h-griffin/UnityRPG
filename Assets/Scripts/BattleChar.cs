using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour{

    public bool isPlayer;
    public string[] movesAvailable;
    
    public string charName;
    public int currentHP, maxHP, currentMP, maxMP, strength, defence, weaponPower, armrPower;
    public bool hasDied;

    //death
    public SpriteRenderer spriteRenderer;
    public Sprite deadSprite;
    public Sprite aliveSprite;

    private bool _shouldFade;
    public float fadeSpeed = 1f;


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_shouldFade)
        {
            // change to red and fade out
            spriteRenderer.color = new Color(
                Mathf.MoveTowards(spriteRenderer.color.r, 1f, fadeSpeed * Time.deltaTime), 
                Mathf.MoveTowards(spriteRenderer.color.g, 0f, fadeSpeed * Time.deltaTime), 
                Mathf.MoveTowards(spriteRenderer.color.b, 0f, fadeSpeed * Time.deltaTime), 
                Mathf.MoveTowards(spriteRenderer.color.a, 0f, fadeSpeed * Time.deltaTime)
                ); 
            if(spriteRenderer.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void EnemyFade()
    {
        _shouldFade = true;
    }
}
