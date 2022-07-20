using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// no monobehaviour - not a component to add to objects

// shows list drp downs in the inspector and can be dupilicated (like a constructor)
[System.Serializable]

public class BattleMove 
{
    // store move variables here
    public string moveName;
    public int movePower;
    public int movecost;
    public AttackEffect theEffect;

}
