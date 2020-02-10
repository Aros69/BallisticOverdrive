using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class AmmoUI : MonoBehaviour
{
	PlayerStats m_playerStats;
	int ammo;

	private void Awake()
	{
		PlayerAmmoUpdated(ClientScene.localPlayer);
		PlayerAnnouncer.OnPlayerStatsUpdated += PlayerAmmoUpdated;
	}

	// Update is called once per frame
	void Update()
	{
		if (m_playerStats != null)
		{
			GetComponent<Text>().text = m_playerStats.Ammo.ToString();
		}
	}

	private void OnDestroy()
	{
		PlayerAnnouncer.OnPlayerStatsUpdated -= PlayerAmmoUpdated;
	}

	void PlayerAmmoUpdated(NetworkIdentity localPlayer)
	{
		if (localPlayer != null)
		{
			m_playerStats = localPlayer.GetComponent<PlayerStats>();
		}

		this.enabled = (localPlayer != null);
	}
}
