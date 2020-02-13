using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileInfo : MonoBehaviour
{
    private GameObject m_owner;
    
    public GameObject Owner{get => m_owner; set {m_owner = value;}}
}
