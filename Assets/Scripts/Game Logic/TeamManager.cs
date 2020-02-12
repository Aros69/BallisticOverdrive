using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team {Red, Blue, Black};

public class TeamManager : MonoBehaviour
{
    [SerializeField]
    private Team m_team;
    private bool m_isProjectile = false;
    static bool m_redTeamFull = false; 

    /// <summary>
    /// Return the team of the Gameobject
    /// </summary>
    /// <returns></returns>
    public Team getTeam()
    {
        return m_team;
    }

    public bool isProjectile()
    {
        return m_isProjectile;
    }

    /// <summary>
    /// Set the team of a gameObject
    /// </summary>
    /// <param name="t"></param>
    public void setTeam(Team t)
    {
        m_team = t;
    }

    /// <Summary>
    /// Set the team and owner of a projectile
    /// </Summary>
    public void setProjectile(Team t, GameObject o)
    {
        m_team = t;
        m_isProjectile = true;
        GetComponent<ProjectileInfo>().Owner = o;
    }

    /// <summary>
    /// Auto assign a team to a **PLAYER** object. for now the first player is always Red and the rest are blue
    /// </summary>
    public void autoAssignTeam()
    {
        if(!m_redTeamFull)
        {
            m_team = Team.Red;
            m_redTeamFull = true;
            GetComponent<HealthManager>().setMaxHP(GameSettings.redTeamHealth);
        }
        else
        {
            m_team = Team.Blue;
            GetComponent<HealthManager>().setMaxHP(GameSettings.blueTeamHealth);
        }
        GetComponent<AmmoManager>().init();
    }

}
