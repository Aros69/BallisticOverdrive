using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject m_player = null;
    [SerializeField] private GameObject m_blueReloadEffect = null;
    [SerializeField] private GameObject m_redReloadEffect = null;

    private GameObject m_activeEffect = null;
    private AudioSource m_audioSrc = null;
    private ShootCommand m_playerShootScript = null;

    [SerializeField] protected GameObject m_projectile = null;
    public GameObject projectile {
        get => m_projectile;
        set { m_projectile = value; }
    }
    [SerializeField] protected Transform m_shootSpawn = null;
    public Transform shootSpawn
    {
        get => m_shootSpawn;
    }

    [SerializeField] protected float m_maxCapacity = 3;
    public float maxCapacity
    {
        get => m_maxCapacity;
    }
    [SerializeField] protected float m_reloadTime = 1;

    private float m_ammo = 3;
    public float ammo { get => m_ammo; }

    //Automatic m_reloading
    private float m_startedReload = 0;

    // return false if unable to shoot
    public bool Shoot()
    {
        //Debug.Log(m_ammo);
        if (m_ammo < 1.0f)
            return false;
        else
        {
            m_ammo -= 1.0f;
            m_playerShootScript.shoot();
            return true;
        }
    }

    int GetAmmoUnit()
    {
        return (int)(m_ammo);
    }
    public void UpdateAmmo()
    {

        if (m_ammo < m_maxCapacity)
        {
            int a = GetAmmoUnit();
            m_ammo += Time.deltaTime/m_reloadTime;
            // If another ammo unit was incremented play sound
            if(GetAmmoUnit() != a )
            {
                m_audioSrc.Play();

                // GameObject o = Instantiate(m_activeEffect, transform.position, transform.rotation);
            }
        } else {
            m_ammo = m_maxCapacity;
        }

        if(m_player.GetComponent<NetworkIdentity>().hasAuthority){
			HUDController.instance.UpdateAmmo(m_ammo);
        }
    }
    
    void Awake()
    {
        m_ammo = m_maxCapacity;
        m_shootSpawn = transform.GetChild(0);
        m_playerShootScript = m_player.GetComponent<ShootCommand>();
        m_audioSrc = GetComponent<AudioSource>();
    }

    void Start()
    {
        if(m_player.GetComponent<NetworkIdentity>().hasAuthority){
            HUDController.instance.SetMaxAmmo((int)m_maxCapacity);
            m_activeEffect = m_player.GetComponent<TeamManager>().getTeam() == Team.Blue ? m_blueReloadEffect : m_redReloadEffect;
        }
    }

	// Update is called once per frame
	void Update()
    { }  
}
