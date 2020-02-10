using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ClientID : MonoBehaviour
{
	PlayerStats m_playerStats;
	int clientID;

	private void Awake()
	{
		PlayerIDUpdated(ClientScene.localPlayer);
		PlayerAnnouncer.OnPlayerStatsUpdated += PlayerIDUpdated;
	}

	// Update is called once per frame
	void Update()
    {
		if (m_playerStats != null)
		{
			GetComponent<Text>().text = ClientScene.localPlayer.netId.ToString();
		}
	}

	private void OnDestroy()
	{
		PlayerAnnouncer.OnPlayerStatsUpdated -= PlayerIDUpdated;
	}

	void PlayerIDUpdated(NetworkIdentity localPlayer)
	{
		if (localPlayer != null)
		{
			m_playerStats = localPlayer.GetComponent<PlayerStats>();
		}
		this.enabled = (localPlayer != null);
	}

}
