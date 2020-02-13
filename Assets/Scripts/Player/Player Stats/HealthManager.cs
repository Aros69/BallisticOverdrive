using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private int m_maxHP = 5;
	private int m_HP = 5;
    public int HP {get => m_HP;}
	public int MaxHP { get => m_maxHP; }
    public void setMaxHP(int m)
    {
        m_maxHP = m;
    }
    public bool takeDamage()
    {
        m_HP--;
		PlayerDie();
		return isDead();
    }

    public bool isDead()
    {
        return m_HP <= 0;
    }

	private void PlayerDie()
	{
		//GameManager.
		//NetworkIdentity identity = GetComponent<NetworkIdentity>();
		//TargetDead(identity.connectionToClient);
	}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            takeDamage();
    }
	

}