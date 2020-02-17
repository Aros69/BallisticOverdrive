using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerCommunication : NetworkBehaviour
{
	private void Start()
	{
		CmdAddPlayer(gameObject);
	}


	[Command]
	public void CmdAddPlayer(GameObject player)
	{
		GameManager.Instance.SrvAddPlayer(player);
	}

	[TargetRpc]
	public void TargetWaitingPlayer(NetworkConnection conn, GameObject player)
	{
		player.GetComponent<PlayerController>().BlockMovement();
		player.GetComponent<PlayerController>().Teleport(new Vector3(0, 0, 0));
	}

	[ClientRpc]
	public void RpcGameStart(Vector3 spawnPosition)
	{
		if (hasAuthority)
		{
			Debug.Log("I teleport the player");
			gameObject.GetComponent<PlayerController>().Teleport(new Vector3(0, 0, 0));
		}
		// unblock player function (TODO)
	}
}
