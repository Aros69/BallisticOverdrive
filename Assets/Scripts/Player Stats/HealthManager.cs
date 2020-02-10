using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float maxHP = 5;
    private float HP = 5;

    public void setMaxHP(int m)
    {
        maxHP = m;
    }
    public bool takeDamage()
    {
        HP--;
        return isDead();
    }

    public bool isDead()
    {
        return HP <= 0;
    }
}