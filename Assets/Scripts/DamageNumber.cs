using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// access to text
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{

    public Text damageText;
    public float lifetime = 1;
    public float moveSpeeed = 1;
    public float placementJitter = .5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);
        transform.position += new Vector3(0f, moveSpeeed * Time.deltaTime, 0f);

    }

    // display damage
    public void setDamage(int damageAmmount)
    {
        damageText.text = damageAmmount.ToString();
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);
    }
}
