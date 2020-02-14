using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class AmmoManager : NetworkBehaviour
{
    [SerializeField] private GameObject m_ammoHUD = null;
    [SerializeField] private Weapon m_weapon = null;
    [SerializeField] private bool m_debug = false;

    public float Ammo
    {
        get => m_weapon.ammo;
    }

    public bool Shoot()
    {
        return m_weapon.Shoot();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(m_ammoHUD != null)
            m_ammoHUD.GetComponent<Renderer>().material.SetFloat("_AmmoMax", m_weapon.maxCapacity);
    }

    void Update()
    {
        m_weapon.UpdateAmmo();
        UpdateAmmo();
        if (isLocalPlayer)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
    }
    public void init()
    {
        Color col = GetComponent<TeamManager>().getTeam().Equals(Team.Blue) ?
                    new Color(0.0f, 0.0f, 1.0f, 1.0f): new Color(1.0f, 0.0f, 0.0f, 1.0f);
        if(m_ammoHUD != null)
        {
            m_ammoHUD.GetComponent<Renderer>().material.SetColor("_Color", col);
            m_ammoHUD.GetComponent<Renderer>().material.SetFloat("_AmmoMax", m_weapon.maxCapacity);
        }
    }

    void UpdateAmmo()
    {
        if(m_ammoHUD != null)
        {    
            // Update HUD
            m_ammoHUD.GetComponent<Renderer>().material.SetFloat("_AmmoLeft", m_weapon.ammo + m_weapon.reloading + m_weapon.delay);
        }
        if(m_debug)
            Print();
    }
    void Print()
    {
        Debug.Log("ammo left : " + m_weapon.ammo + "\nm_reloading : " + m_weapon.reloading);
    }
}
