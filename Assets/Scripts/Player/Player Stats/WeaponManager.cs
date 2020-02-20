using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class WeaponManager : MonoBehaviour
{
	[SerializeField] private GameObject m_weaponSimple;
	[SerializeField] private GameObject m_weaponSpray;
	private GameObject _currentWeapon;
	private GameObject[] _allweapons;

	public void Start()
	{
		_currentWeapon = m_weaponSimple;
		GetComponent<ShootCommand>().setWeapon(_currentWeapon.GetComponent<Weapon>());
		GetComponent<AmmoManager>().setWeapon(_currentWeapon.GetComponent<Weapon>());
		
		_allweapons = new GameObject[2] { m_weaponSimple, m_weaponSpray };
	}

	public void SetWeapon(WeaponType weapon)
	{
		
		switch (weapon)
		{
			case WeaponType.simple:
				_currentWeapon = m_weaponSimple;
				break;
			case WeaponType.spray:
				_currentWeapon = m_weaponSpray;
				break;
			default:
				Debug.Log("there is no weapon");
				break;
		}

		foreach (GameObject o in _allweapons)
		{
			if (o != _currentWeapon)
			{
				o.SetActive(false);
			}
		}
		
		GetComponent<ShootCommand>().setWeapon(_currentWeapon.GetComponent<Weapon>());
		GetComponent<AmmoManager>().setWeapon(_currentWeapon.GetComponent<Weapon>());
	}
}
