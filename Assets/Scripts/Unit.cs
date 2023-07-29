using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;
    public int speed;

    public int maxHP;
    public int currentHP;

    public int maxEnergy;
    public int currentEnergy;

    public bool TakeDamage(int dmg) 
    {
        currentHP -= dmg;

        if(currentHP <= 0) return true;
        
        return false;
    }
}
