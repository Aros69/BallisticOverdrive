using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HealthUI : MonoBehaviour
{
	HealthManager m_healthManager;
	GameObject [] m_healthBar;

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
			for (int i = 0; i < m_healthManager.MaxHP; i++)
			{
				if (m_healthManager.HP > i)
				{
					m_healthBar[i].transform.Find("Foreground").GetComponent<Image>().color = new Color(255, 0, 0, 226);
				}
				else
				{
					m_healthBar[i].transform.Find("Foreground").GetComponent<Image>().color = new Color(0, 0, 0, 255);
				}
			}
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
			float pos_x = 0;
			m_healthManager = localPlayer.GetComponent<HealthManager>();

			m_healthBar = new GameObject[m_healthManager.MaxHP];
			for (int i = 0; i < m_healthManager.MaxHP; i ++)
			{
				m_healthBar[i] = Instantiate(Resources.Load("UI/HealthBar"), transform) as GameObject;
				m_healthBar[i].transform.position += new Vector3(pos_x,0,0);
				pos_x += 50;
			}
			Debug.Log("hello " + m_healthManager.HP);
		}

		this.enabled = (localPlayer != null);
	}
}
