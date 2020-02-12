using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class DeathUI : MonoBehaviour
{
	HealthManager m_healthManager;
	GameObject deathScreen;

	private void Awake()
	{
		PlayerDead(ClientScene.localPlayer);
		PlayerAnnouncer.OnPlayerStatsUpdated += PlayerDead;
	}

    // Update is called once per frame
    void Update()
    {
		if (m_healthManager != null)
		{
			if (m_healthManager.isDead())
			{
				deathScreen = Instantiate(Resources.Load("UI/DeathScreen"), transform) as GameObject;
			}
		}

	}

	private void OnDestroy()
	{
		PlayerAnnouncer.OnPlayerStatsUpdated -= PlayerDead;
	}

	void PlayerDead(NetworkIdentity localPlayer)
	{
		if (localPlayer != null)
		{
			m_healthManager = localPlayer.GetComponent<HealthManager>();

		}

		this.enabled = (localPlayer != null);
	}
}
