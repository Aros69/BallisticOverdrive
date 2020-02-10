using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HealthUI : MonoBehaviour
{
	HealthManager m_healthManager;

	private void Awake()
	{
		PlayerHealthUpdated(ClientScene.localPlayer);
		PlayerAnnouncer.OnPlayerStatsUpdated += PlayerHealthUpdated;
	}

	// Update is called once per frame
	void Update()
    {
		if (m_healthManager != null)
		{
			GetComponent<Text>().text = m_healthManager.HP.ToString();
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
			m_healthManager = localPlayer.GetComponent<HealthManager>();
		}

		this.enabled = (localPlayer != null);
	}
}
