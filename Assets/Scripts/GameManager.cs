﻿using System.Collections;
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

public enum WeaponType
{
	simple,
	spray
}

[System.Serializable]
public struct PlayerProfile
{
	//const float Speed = 5.0f;
	//const float LookSensitivity = 3.0f;
	//const float Jumpforce = 500;
	//const float AirDrag = 1.001f;
	//const float GroundDrag = 1.2f;
	//const float AirControl = 0.05f;
	//const float BonusGravity = 2.0f;
	public int MaxLife;
	public WeaponType weaponType;
}

public class GameManager : NetworkBehaviour
{
	private static GameManager _gameManager;
	public static GameManager Instance { get { return _gameManager; } }

	[SerializeField]
	private PlayerProfile _AttackPlayer;
	[SerializeField]
	private PlayerProfile _DefensePlayer;

	private int[] _alivePlayers; // number of player alive seperate in team
	[SerializeField] private int _maxPlayerTeam; // max player by team (use only if team same size)
	[SerializeField] private int _maxPlayer;
	private GameObject _resultScreen;

	private List<GameObject> _playersLists;
	private List<GameObject>[] _teamLists; //2D array of player seperate in team
	private Team _attack_side;
	private Team _defense_side;
	GameState state;
	private int timer;
	

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
			_teamLists = new List<GameObject>[(int)Team.Nb - 1];
			for (int i = 0; i < (int)Team.Nb - 1; i++)
			{
				_teamLists[i] = new List<GameObject>();
			}
			_maxPlayerTeam = 2;
			_maxPlayer = 3;
			state = GameState.waiting;
			timer = 0;
		}

		//Debug.Log("network identity : " + gameObject.GetComponent<NetworkIdentity>().netId);
	}

	private void Update()
	{
		//if (timer%1000 == 0)
		//{
		//	SrvCheckPlayer();
		//}
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


		///////////// if team has same size (with spectator)
		/*
		for (int i = 0; i < (int)Team.Nb-1; i++)
		{
			for (int j = 0; j < _maxPlayerTeam; j++)
			{
				_teamLists[i].Add(_playersLists[i* _maxPlayerTeam + j]);
				_playersLists[i * _maxPlayerTeam + j].GetComponent<TeamManager>().setTeam((Team)i);
			}
			_alivePlayers[i] = _maxPlayerTeam;
		}
		*/

		// first player on attack_side
		_teamLists[(int)_attack_side].Add(_playersLists[0]);
		_playersLists[0].GetComponent<TeamManager>().setTeam(_attack_side);
		_alivePlayers[(int)_attack_side] = 1;
		for (int i = 1; i < _playersLists.Count; i++)
		{
			_teamLists[(int)_defense_side].Add(_playersLists[i]);
			_playersLists[i].GetComponent<TeamManager>().setTeam(_defense_side);
		}
		_alivePlayers[(int)_defense_side] = _playersLists.Count - 1;
	}

	[Server]
	public void SrvAddPlayer(GameObject player)
	{
		AddPlayer(player);

		player.GetComponent<ServerCommunication>().TargetWaitingPlayer(player.GetComponent<NetworkIdentity>().connectionToClient, player);

		if (_playersLists.Count == _maxPlayerTeam * ((int)Team.Nb - 1) || _playersLists.Count == _maxPlayer)
		{
			if (state == GameState.waiting)
			{
				SrvGameStart();
			}
			// else ajouter le joueur en plein partie?
		}
	}


	[Server]
	private void SrvGameStart()
	{
		state = GameState.play;
		AssignSide();

		//organize spawn object
		List<PlayerSpawn>[] spawns = new List<PlayerSpawn>[2];
		spawns[0] = new List<PlayerSpawn>(); spawns[1] = new List<PlayerSpawn>();

		foreach (PlayerSpawn spawn in GameObject.FindObjectsOfType<PlayerSpawn>())
		{
			if (spawn.SpawnType == SpawnType.attacker) spawns[(int)_attack_side].Add(spawn);
			else spawns[(int)_defense_side].Add(spawn);
		}

		// AttackTeam
		for (int i = 0; i < _teamLists[(int)_attack_side].Count; i++)
		{
			Vector3 spawnPos = spawns[(int)_attack_side][i].transform.position;
			_teamLists[(int)_attack_side][i].GetComponent<ServerCommunication>().RpcGameStart(spawnPos, _attack_side, _AttackPlayer);
		}

		// DefenseTeam
		for (int i = 0; i < _teamLists[(int)_defense_side].Count; i++)
		{
			Vector3 spawnPos = spawns[(int)_defense_side][i].transform.position;
			_teamLists[(int)_defense_side][i].GetComponent<ServerCommunication>().RpcGameStart(spawnPos, _defense_side, _DefensePlayer);
		}

		Debug.Log("red player " + _alivePlayers[(int)Team.Red]);
		Debug.Log("blue player " + _alivePlayers[(int)Team.Blue]);
	}

	[Server]
	public void SrvPlayerDie(GameObject player)
	{
		Team teamPlayer = player.GetComponent<TeamManager>().getTeam();
		_alivePlayers[(int)teamPlayer]--;
		Debug.Log("player " + player.GetComponent<NetworkIdentity>().netId + " is dead");
		
		if (_alivePlayers[(int)teamPlayer] == 0)
		{
			state = GameState.result;
			Team teamLoser = teamPlayer;
			Team teamWinner = (Team)(((int)teamLoser + 1) % 2);
			Debug.Log("winner team is " + teamWinner);
			Debug.Log("loser team is " + teamLoser);

			player.GetComponent<ServerCommunication>().RpcSetWinner(teamWinner);
		}
	}
	
	// TODO recheck if list remove work well with game object
	[Server]
	public void SrvPlayerLeave(GameObject player)
	{
		_playersLists.Remove(player);
		for (int i = 0; i < (int)Team.Nb - 1; i++)
		{
			if (_teamLists[i].IndexOf(player) != -1)
			{
				_teamLists[i].Remove(player);
				_alivePlayers[i]--;
			}
		}
	}

	// TODO not done
	[Server]
	public void SrvCheckPlayer()
	{
		for (int i = 0; i < (int)Team.Nb - 1; i++)
		{
			int nbPlayer = _teamLists[i].Count;
			for (int j = 0; j < nbPlayer; j++)
			{
				if (_teamLists[i][j] == null)
				{
					_teamLists[i].RemoveAt(j);
				}
			}
		}
	}
}