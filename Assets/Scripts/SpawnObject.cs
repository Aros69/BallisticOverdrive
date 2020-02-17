using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnObject : NetworkBehaviour
{

	[SerializeField]
	private GameObject _GameManager;

	//public override void OnStartLocalPlayer()
	//{
	//	base.OnStartLocalPlayer();
	//	CmdSpawnGameManager();
	//}

	public override void OnStartClient()
	{
		base.OnStartClient();
		Debug.Log("Call GpawnGameManager: is client " + isLocalPlayer);
		CmdSpawnGameManager();
	}

	[Command]
	public void CmdSpawnGameManager()
	{
		Debug.Log("Spawn game manager");
		GameObject gm = Instantiate(_GameManager, new Vector3(), Quaternion.identity);
		NetworkServer.Spawn(gm, GetComponent<NetworkIdentity>().connectionToClient);

		if (GameManager.Instance != null)
		{
			GameManager.Instance.CmdGameInit(gameObject);
			
		} else
		{
			Debug.Log("GameManger is null (from SpawnObject)");
		}
	}
}
