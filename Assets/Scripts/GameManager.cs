using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
	private static GameManager _gameManager;
	public static GameManager Instance { get { return _gameManager; } }
	
	private int [] _teamNB;
	private GameObject _resultScreen;
	private ArrayList _players_lists;

	public int getTeamNB(Team id)
	{
		Debug.Log("getTeam " + _teamNB[(int)id]);
		return _teamNB[(int)id];
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

			_teamNB = new int[(int)Team.Nb];
			_players_lists = new ArrayList();
		}
	}

	private void Update()
	{
		//if (Input.GetKeyDown(KeyCode.Space) &&)
		//{
		//	GetComponent<AudioSource>().Play();
		//	m_rigidBody.AddForce(Vector3.up * m_jumpforce);
		//	CmdTest();
		//	GameManager.Instance.AddPlayer(Team.Red);

		//}
		Debug.Log("update GameManager");
	}


	//mettre a jour avec spaw coter client?
	public void AddPlayer(GameObject player)
	{
		Debug.Log("client: add player");
		

		CmdAddPlayer(player);
		Debug.Log("network identity : " + gameObject.GetComponent<NetworkIdentity>().netId);
	}

	public void PlayerDie(NetworkIdentity localPlayer)
	{
		//CmdPlayerDie(localPlayer);
	}

	[Command]
	private void CmdAddPlayer(GameObject player)
	{
	
		Debug.Log("Server: add player");

		_players_lists.Add(player);
		Team teamID = player.GetComponent<TeamManager>().getTeam();
		_teamNB[(int)teamID]++;

		//update all game Manager
		RpcAddPlayer(player);
	}


	//[Command]
	//private void CmdPlayerDie(NetworkIdentity localPlayer)
	//{
	//	Team id = localPlayer.GetComponent<TeamManager>().getTeam();

	//	TargetPlayerDie(localPlayer.connectionToClient);
	//	RpcRemovePlayer(id);
	//}

	[ClientRpc]
	public void RpcAddPlayer(GameObject player)
	{
		Team teamID = player.GetComponent<TeamManager>().getTeam();
		_teamNB[(int)teamID]++;
		Debug.Log("yay a new member for team " + teamID.ToString());
	}

	//[TargetRpc]
	//private void TargetPlayerDie(NetworkConnection conn)
	//{
	//	Debug.Log("he is dead " + conn.connectionId);
	//}

	//[ClientRpc]
	//private void RpcRemovePlayer(Team id)
	//{
	//	Debug.Log("we lose someone here " + id.ToString());

	//	_teamNB[(int)id]--;
	//	if (_teamNB[(int)id] == 0)
	//	{
	//		RpcTeamWin(id);
	//	}
	//}

	//// TODO code to change, find a way to dynamise UI?
	//[ClientRpc]
	//private void RpcTeamWin(Team winner)
	//{
	//	Debug.Log("the winner is " + winner.ToString());
	//	Team ownTeam = ClientScene.localPlayer.GetComponent<TeamManager>().getTeam();
	//	if (ownTeam == winner)
	//	{
	//		_resultScreen = Instantiate(Resources.Load("UI/WinnerScreen"), GameObject.Find("Canvas").transform) as GameObject;
	//	}
	//	else
	//	{
	//		_resultScreen = Instantiate(Resources.Load("UI/LooserScreen"), GameObject.Find("Canvas").transform) as GameObject;
	//	}
	//}

}