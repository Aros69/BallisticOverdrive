using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class PlayerStats : NetworkBehaviour
{

	[Header("Players stats")]
	[SerializeField]
	private int m_health;
	private int m_ammo;

	public int Health { get => m_health; set => m_health = value; }
	public int Ammo { get => m_ammo; set => m_ammo = value; }

	// Start is called before the first frame update
	void Start()
    {
		m_health = 1;
		m_ammo = 2;
    }

    // Update is called once per frame
    void Update()
    {
		//if (Time.frameCount%120 == 0) m_health++;
    }

	public void LoseHealth(int value = 1)
	{
		Health -= value;
		//PlayerAnnouncer.OnPlayerStatsUpdated 
	}
}
