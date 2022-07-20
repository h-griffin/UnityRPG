using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//access to text
using UnityEngine.UI;

public class BattleNotification : MonoBehaviour
{

    public float awakeTime;
    private float _awakeCounter;
    public Text theText;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_awakeCounter > 0)
        {
            _awakeCounter -= Time.deltaTime;
           
            if(_awakeCounter <= 0)
            {
                gameObject.SetActive(false);

            }
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _awakeCounter = awakeTime;
    }
}
