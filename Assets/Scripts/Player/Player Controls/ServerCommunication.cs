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
		player.GetComponent<PlayerController>().Teleport(new Vector3(0, 2.0f, 0));
		HUDController.instance.SetMode(HUDMode.waitingForPlayer);
	}

	[ClientRpc]
	public void RpcGameStart(Vector3 spawnPosition, Team teamColor, PlayerProfile profile)
	{
		if (hasAuthority)
		{
			gameObject.GetComponent<PlayerController>().Teleport(spawnPosition);

			// permet executer 1 fois
			HUDController.instance.SetMode(HUDMode.playing);
		}
		gameObject.GetComponent<HealthManager>().setMaxHP(profile.MaxLife);
		gameObject.GetComponent<TeamManager>().setTeam(teamColor);

		// unblock player function (TODO)
	}

	[ClientRpc]
	public void RpcSetWinner(Team winner)
	{
		if (winner == Team.Red)
			HUDController.instance.SetMode(HUDMode.redTeamVictory);
		else
			HUDController.instance.SetMode(HUDMode.blueTeamVictory);
	}
}
