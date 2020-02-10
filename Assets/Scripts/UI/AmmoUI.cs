using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class AmmoUI : MonoBehaviour
{
	AmmoManager m_ammoManager;

	private void Awake()
	{
		PlayerAmmoUpdated(ClientScene.localPlayer);
		PlayerAnnouncer.OnPlayerStatsUpdated += PlayerAmmoUpdated;
	}

	// Update is called once per frame
	void Update()
	{
		if (m_ammoManager != null)
		{
			GetComponent<Text>().text = m_ammoManager.Ammo.ToString();
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
			m_ammoManager = localPlayer.GetComponent<AmmoManager>();
		}

		this.enabled = (localPlayer != null);
	}
}
