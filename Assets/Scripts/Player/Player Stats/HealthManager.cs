using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HealthManager : NetworkBehaviour
{
    [SerializeField] private int m_maxHP;
	public int m_HP;
    public int HP {get => m_HP;}
	public int MaxHP { get => m_maxHP; }
	// ATTENTION: cette fonction est supposé appelé qu'une fois
	public void setMaxHP(int m)
    {
        m_maxHP = m;
		m_HP = m;
		if (hasAuthority)
			HUDController.instance.SetMaxLife(m);
    }
    public bool takeDamage()
    {
        m_HP--;
		if (hasAuthority)
		{
			Debug.Log("no authority");
			HUDController.instance.UpdateLife(m_HP);
		}

		return isDead();
    }

    public bool isDead()
    {
		if (m_HP <= 0 && hasAuthority)
		{
			PlayerDie();
		}
        return m_HP <= 0;
    }

	private void PlayerDie()
	{
		if (gameObject.GetComponent<NetworkIdentity>().isClient)
		{
			GetComponent<ServerCommunication>().CmdPlayerDie();
		}
	}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            takeDamage();
    }
	

}