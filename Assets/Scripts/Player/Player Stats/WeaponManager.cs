using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponManager : MonoBehaviour
{
	[SerializeField] private GameObject m_weaponSimple;
	[SerializeField] private GameObject m_weaponSpray;
	private GameObject m_currentWeapon;
	private GameObject[] _allweapons;

	public GameObject WeaponSimple { get => m_weaponSimple; }

	public void Start()
	{
		m_currentWeapon = WeaponSimple;
		GetComponent<ShootCommand>().setWeapon(m_currentWeapon.GetComponent<Weapon>());
		GetComponent<AmmoManager>().setWeapon(m_currentWeapon.GetComponent<Weapon>());
		
		_allweapons = new GameObject[2] { WeaponSimple, m_weaponSpray };
	}
	
	// only works if it's call at start
	public void SetWeapon(WeaponType weapon)
	{
		
		switch (weapon)
		{
			case WeaponType.simple:
				m_currentWeapon = WeaponSimple;
				break;
			case WeaponType.spray:
				m_currentWeapon = m_weaponSpray;
				break;
			default:
				Debug.Log("there is no weapon");
				break;
		}

		foreach (GameObject o in _allweapons)
		{
			if (o != m_currentWeapon)
			{
				o.SetActive(false);
			}
		}
		
		GetComponent<ShootCommand>().setWeapon(m_currentWeapon.GetComponent<Weapon>());
		GetComponent<AmmoManager>().setWeapon(m_currentWeapon.GetComponent<Weapon>());

		if (GetComponent<NetworkIdentity>().hasAuthority)
			HUDController.instance.SetMaxAmmo((int)m_currentWeapon.GetComponent<Weapon>().maxCapacity);
	}
}
