using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManagerMegaBullet : TeamManager
{
	public override void setTeam(Team t)
	{
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			gameObject.transform.GetChild(i).GetComponent<TeamManager>().setTeam(t);
		}
	}
}
