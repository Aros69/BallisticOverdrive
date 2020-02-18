using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerCommunication : NetworkBehaviour
{
	private void Start()
	{
		if (isClient)
		{
			CmdAddPlayer();
		}
	}


	[Command]
	public void CmdAddPlayer()
	{
		
		GameManager.Instance.SrvAddPlayer(gameObject);
	}

	[Command]
	public void CmdPlayerDie()
	{
		Debug.Log("some is dead here");
		GameManager.Instance.SrvPlayerDie(gameObject);
	}

	[Command]
	public void CmdPlayerLeave()
	{
		GameManager.Instance.SrvPlayerLeave(gameObject);
	}

	[TargetRpc]
	public void TargetWaitingPlayer(NetworkConnection conn, GameObject player)
	{
		player.GetComponent<PlayerController>().BlockMovement();
		player.GetComponent<PlayerController>().Teleport(new Vector3(0, 0, 0));
	}

	[ClientRpc]
	public void RpcGameStart(Vector3 spawnPosition, Team teamColor)
	{
		if (hasAuthority)
		{
			Debug.Log("I teleport the player to " + spawnPosition);
			gameObject.GetComponent<PlayerController>().Teleport(spawnPosition);
			if (teamColor == Team.Red) HUDController.instance.SetPlayerColor(Color.red);
			if (teamColor == Team.Blue) HUDController.instance.SetPlayerColor(Color.blue);
		}
		// unblock player function (TODO)
	}

	[ClientRpc]
	public void RpcSetWinner(Team winner)
	{
		Debug.Log("set winner");
		if (winner == Team.Red)
			HUDController.instance.SetMode(HUDMode.redTeamVictory);
		else
			HUDController.instance.SetMode(HUDMode.blueTeamVictory);
	}
}
