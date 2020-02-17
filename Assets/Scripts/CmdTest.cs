using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CmdTest : NetworkBehaviour
{
	private int a = 0;


	// Update is called once per frame
	void Update()
    {
		a++;
		//if (a % 500 == 0)
		//{
		//	CmdCallServer();
		//}
	}

	public void CallServer()
	{
		
		Debug.Log("CmdTest: client here, going to call server");
		Debug.Log("Server ? " + isServer);
		CmdCallServer();
	}

	[Command]
	public void CmdCallServer()
	{
		Debug.Log("CmdTest: server here");
	}

	[Server]
	public void SrvCallServer()
	{
		Debug.Log("server only call");
		GameObject.Find("Player_Merge(Clone)").GetComponent<PlayerController>().RpcCall();
	}
	
}
