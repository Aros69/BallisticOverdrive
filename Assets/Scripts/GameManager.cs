using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum GameState
{
	waiting,
	play,
	result,
	endgame,
}

public class GameManager : NetworkBehaviour
{
	private static GameManager _gameManager;
	public static GameManager Instance { get { return _gameManager; } }

	public int[] _alivePlayers; // number of player alive seperate in team
	private int _maxPlayer; // max player by team
	private GameObject _resultScreen;

	private List<GameObject> _playersLists;
	private List<GameObject>[] _TeamLists; //2D array of player seperate in team
	private Team _attack_side;
	private Team _defense_side;
	

	public int getTeamNB(Team id)
	{
		Debug.Log("getTeam " + _alivePlayers[(int)id]);
		return _alivePlayers[(int)id];
	}
	

	private void Awake()
	{
		if (_gameManager != null && _gameManager != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_gameManager = this;

			// init
			_alivePlayers = new int[(int)Team.Nb - 1];
			_playersLists = new List<GameObject>();
			_TeamLists = new List<GameObject>[(int)Team.Nb - 1];
			for (int i = 0; i < (int)Team.Nb - 1; i++)
			{
				_TeamLists[i] = new List<GameObject>();
			}
			_maxPlayer = 1;
		}

		//Debug.Log("network identity : " + gameObject.GetComponent<NetworkIdentity>().netId);
	}

	public void PlayerDie(GameObject player)
	{
		CmdPlayerDie(player);
	}

	private void GameInit(GameObject player)
	{
		Debug.Log("GameInit is server " + isServer);
		CmdGameInit(player);
	}

	private void AddPlayer(GameObject player)
	{
		_playersLists.Add(player);
		Debug.Log("player list : " + _playersLists.Count);
	}

	// TODO shuffle function (and give random to parameter so everyone has same random)
	private void AssignSide()
	{
		_attack_side = Team.Red;
		_defense_side = Team.Blue;
		

		for (int i = 0; i < (int)Team.Nb-1; i++)
		{
			for (int j = 0; j < _maxPlayer; j++)
			{
				_TeamLists[i].Add(_playersLists[i* _maxPlayer + j]);
				_playersLists[i * _maxPlayer + j].GetComponent<TeamManager>().setTeam((Team)i);
			}
			_alivePlayers[i] = _maxPlayer;
		}
	}


	[Server]
	public void CmdGameInit(GameObject player)
	{
		Debug.Log("CmdGameInit: doing some init here");
		SrvAddPlayer(player);
	}

	[Server]
	private void SrvAddPlayer(GameObject player)
	{
		AddPlayer(player);

		//update all game Manager
		RpcAddPlayer(player);
		TargetWaitingPlayer(player.GetComponent<NetworkIdentity>().connectionToClient, player);

		if (_playersLists.Count == _maxPlayer * ((int)Team.Nb - 1))
		{
			Debug.Log("game start");
			SrvGameStart();
		}
	}

	[ClientRpc]
	public void RpcAddPlayer(GameObject player)
	{
		AddPlayer(player);
		//Debug.Log("RpcAddplayer " + teamID.ToString());
	}

	[TargetRpc]
	private void TargetWaitingPlayer(NetworkConnection conn, GameObject player)
	{
		// this is alway true but who knows
		player.GetComponent<PlayerController>().BlockMovement();
		player.GetComponent<PlayerController>().Teleport(new Vector3(0, 0, 0));
		
	}

	[Server]
	private void SrvGameStart()
	{
		AssignSide();

		RpcGameStart();

		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				TargetGameStart();
			}
		}

		Debug.Log("my id " + base.netId);
		Debug.Log("red player " + _alivePlayers[(int)Team.Red]);
		Debug.Log("blue player " + _alivePlayers[(int)Team.Blue]);
	}

	[ClientRpc]
	private void RpcGameStart()
	{
		AssignSide();

		Debug.Log("my id " + base.netId);
		Debug.Log("red player " + _TeamLists[(int)Team.Red].Count);
		Debug.Log("blue player " + _TeamLists[(int)Team.Blue].Count);
	}

	[TargetRpc]
	private void TargetGameStart()
	{

	}



	[Command]
	public void CmdPlayerDie(GameObject player)
	{
		Debug.Log("Serveur: player die");
		Team teamID = player.GetComponent<TeamManager>().getTeam();
		_alivePlayers[(int)teamID]--;

		// dispatch
		RpcPlayerDie(teamID);

		// Check Winner and loser
		if (_alivePlayers[(int)teamID] == 0)
		{
			// Supposing if there are only two team (TODO recheck %2
			Team teamLoser = teamID;
			int i = (int)teamLoser;
			Team teamWinner = (Team)((i + 1) % 2);

			RpcTeamWin(teamWinner);
		}
	}
	//[Command]
	//public void CmdPlayerDie(NetworkIdentity localPlayer)
	//{
		//Team id = localPlayer.GetComponent<TeamManager>().getTeam();

		//TargetPlayerDie(localPlayer.connectionToClient);
		//RpcRemovePlayer(id);
	//}







	//[TargetRpc]
	//private void TargetPlayerDie(NetworkConnection conn)
	//{
	//	Debug.Log("he is dead " + conn.connectionId);
	//}

	[ClientRpc]
	private void RpcPlayerDie(Team id)
	{
		Debug.Log("we lose someone here " + id.ToString());

		_alivePlayers[(int)id]--;
	}
	
	[ClientRpc]
	private void RpcTeamWin(Team winner)
	{
		Debug.Log("the winner is " + winner.ToString());
		//Team ownTeam = ClientScene.localPlayer.GetComponent<TeamManager>().getTeam();
		//if (ownTeam == winner)
		//{
		//	_resultScreen = Instantiate(Resources.Load("UI/WinnerScreen"), GameObject.Find("Canvas").transform) as GameObject;
		//}
		//else
		//{
		//	_resultScreen = Instantiate(Resources.Load("UI/LooserScreen"), GameObject.Find("Canvas").transform) as GameObject;
		//}
	}

}