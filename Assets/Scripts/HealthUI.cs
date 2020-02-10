using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HealthUI : MonoBehaviour
{
	PlayerStats m_playerStats;
	int m_health;

	private void Awake()
	{
		PlayerHealthUpdated(ClientScene.localPlayer);
		PlayerAnnouncer.OnPlayerStatsUpdated += PlayerHealthUpdated;
	}

	// Update is called once per frame
	void Update()
    {
		if (m_playerStats != null)
		{
			GetComponent<Text>().text = m_playerStats.Health.ToString();
		}
	}

	private void OnDestroy()
	{
		PlayerAnnouncer.OnPlayerStatsUpdated -= PlayerHealthUpdated;
	}

	void PlayerHealthUpdated(NetworkIdentity localPlayer)
	{
		if (localPlayer != null)
		{
			m_playerStats = localPlayer.GetComponent<PlayerStats>();
		}

		this.enabled = (localPlayer != null);
	}
}
