using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [Header("Teams parameters")]
    [SerializeField]
    public static int redTeamHealth = 5;
    [SerializeField]
    public static int blueTeamHealth = 1;
}
