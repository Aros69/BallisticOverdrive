using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

[RequireComponent(typeof(PlayerStats))]
public class PlayerAnnouncer : NetworkBehaviour
{
	public static event Action<NetworkIdentity> OnPlayerStatsUpdated;
    

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		OnPlayerStatsUpdated?.Invoke(base.netIdentity);
		Debug.Log("announcer init");
	}

	// Update is called once per frame
	void Update()
    {
    }

	private void OnDestroy()
	{
		if (base.isLocalPlayer)
		{
			OnPlayerStatsUpdated?.Invoke(null);
		}
	}
}
