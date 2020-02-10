using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    [SerializeField] private GameObject m_ammoHUD;
    [SerializeField] private GameObject m_projectile;
    [SerializeField] private GameObject m_shootSpawn;
    [SerializeField] private bool m_debug = false;
    [SerializeField] private float m_maxCapacity = 3;
    [SerializeField] private float m_reloadTime = 1;
    [SerializeField] private float m_delay = 0.5f;
    private float m_ammo = 3;

    //Automatic m_reloading
    private float m_reloading = 0;
    private float m_startedReload = 0;

    // return false if unable to shoot
    public bool Shoot()
    {
        if(m_ammo == 0)
            return false;
        else
        {
            Instantiate(m_projectile, m_shootSpawn.transform.position, m_shootSpawn.transform.rotation);
            m_ammo--;
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_ammo = m_maxCapacity;

        if(m_ammoHUD != null)
            m_ammoHUD.GetComponent<Renderer>().material.SetFloat("_AmmoMax", m_maxCapacity);
    }

    void Update()
    {
        UpdateAmmo();
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    public void init()
    {
        m_ammo = m_maxCapacity;
        Color col = GetComponent<TeamManager>().getTeam().Equals(Team.Blue) ?
                    new Color(0.0f, 0.0f, 1.0f, 1.0f): new Color(1.0f, 0.0f, 0.0f, 1.0f);
        if(m_ammoHUD != null)
        {
            m_ammoHUD.GetComponent<Renderer>().material.SetColor("_Color", col);
            m_ammoHUD.GetComponent<Renderer>().material.SetFloat("_AmmoMax", m_maxCapacity);
        }
    }

    void UpdateAmmo()
    {
        if(m_ammo < m_maxCapacity)
        {
            // We want to keep track of when we started m_reloading current bullet, set to time if not already the case
            if(m_startedReload == 0 )
                m_startedReload = Time.time;
            
            m_reloading = (Time.time - m_startedReload)/m_reloadTime - m_delay;
            if(m_reloading > 1.0)
            {
                m_ammo++;
                m_startedReload = 0;
                m_reloading = 0;
            }
        }

        if(m_ammoHUD != null)
        {    
            // Update HUD
            m_ammoHUD.GetComponent<Renderer>().material.SetFloat("_AmmoLeft", m_ammo + m_reloading + m_delay);
        }
        if(m_debug)
            Print();
    }
    void Print()
    {
        Debug.Log("ammo left : " + m_ammo + "\nm_reloading : " + m_reloading);
    }
}
