using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum Team {Red, Blue, Black, Nb};
// Network behaviour because it need GameManager to be launch
public class TeamManager: NetworkBehaviour
{
    [SerializeField] private GameObject m_redVisual;
    [SerializeField] private GameObject m_blueVisual;

	[SerializeField]
	//[SyncVar]
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
    public virtual void setTeam(Team t)
    {
        m_team = t;

        if(m_team == Team.Red){
            if(m_blueVisual != null){
                m_blueVisual.SetActive(false);
                m_redVisual.SetActive(true);
            }
            HUDController.instance.SetPlayerColor(Color.red);
        } else {
            if(m_redVisual != null){
                m_redVisual.SetActive(false);
                m_blueVisual.SetActive(true);
            }
            HUDController.instance.SetPlayerColor(Color.blue);
        }
    }

    /// <Summary>
    /// Set the team and owner of a projectile
    /// </Summary>
    public void setProjectile(Team t, GameObject o)
    {
        setTeam(t);
        m_isProjectile = true;
        GetComponent<ProjectileInfo>().Owner = o;
    }

    /// <summary>
    /// Auto assign a team to a **PLAYER** object. for now the first player is always Red and the rest are blue
	/// ps: If the function is no more determinist, call Bao
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

    void Start()
    {
        m_team = Team.Black;
    }
}
