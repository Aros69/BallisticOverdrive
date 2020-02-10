using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team {Red, Blue, Black};

public class TeamManager : MonoBehaviour
{
    [SerializeField]
    private Team team;

    static bool red_team_full = false; 
    public Team getTeam()
    {
        return team;
    }

    public void setTeam(Team t)
    {
        team = t;
    }

    // For now the first player is always Red and the rest are blue
    public void autoAssignTeam()
    {
        if(!red_team_full)
        {
            team = Team.Red;
            red_team_full = true;
            GetComponent<HealthManager>().setMaxHP(GameSettings.redTeamHealth);
        }
        else
        {
            team = Team.Blue;
            GetComponent<HealthManager>().setMaxHP(GameSettings.blueTeamHealth);
        }
        GetComponent<AmmoManager>().init();
    }

    void Start()
    {
        autoAssignTeam();
    }
}
