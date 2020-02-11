using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{
    generic,
    attacker, 
    defender
}

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private SpawnType spawnType;
    private void OnDrawGizmos()
    {
        if (spawnType == SpawnType.attacker) {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        } else if(spawnType == SpawnType.defender) { 
            Gizmos.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
        } else { 
            Gizmos.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        }

        Gizmos.DrawCube(transform.position, new Vector3(.6f, 1.8f, .6f));
    }
}
