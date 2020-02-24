using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerCommunication : NetworkBehaviour
{
	[SerializeField] private GameObject ghostPrefab = null;
	private IEnumerator ShowResultScreen(float time, Team winner)
	{
		yield return new WaitForSeconds(time);
		if (winner == Team.Red)
			HUDController.instance.SetMode(HUDMode.redTeamVictory);
		else
			HUDController.instance.SetMode(HUDMode.blueTeamVictory);
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

	[ClientRpc]
	public void RpcPlayerDie()
	{
		if (hasAuthority)
		{
			HUDController.instance.SetMode(HUDMode.playerDead);
		}

		// player ghost mode
		gameObject.GetComponent<PlayerController>().enabled = false;
		
		gameObject.GetComponent<PlayerInit>().CmdDollify();
		if(isLocalPlayer)
			Instantiate(ghostPrefab, transform.position, transform.rotation);
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
		player.GetComponent<PlayerController>().Teleport(new Vector3(0, 2.5f, 0));
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
			HUDController.instance.StartTimer();
		}
		gameObject.GetComponent<HealthManager>().setMaxHP(profile.MaxLife);
		gameObject.GetComponent<WeaponManager>().SetWeapon(profile.weaponType);
		gameObject.GetComponent<TeamManager>().setTeam(teamColor);

		// unblock player function (TODO)
	}

	[ClientRpc]
	public void RpcSetWinner(Team winner)
	{
		StartCoroutine(ShowResultScreen(2.5f, winner));
	}

	[Command]
	public void CmdPlayerHit(GameObject player)
	{
		GameManager.Instance.SrvPlayerGetHit(player);

	}

	[ClientRpc]
	public void RpcPlayerGetHit()
	{
		GetComponent<HealthManager>().takeDamage();
	}
}
