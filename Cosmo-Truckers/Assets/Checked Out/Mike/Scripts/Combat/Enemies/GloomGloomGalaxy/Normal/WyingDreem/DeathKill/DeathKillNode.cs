using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathKillNode : MonoBehaviour
{
    [SerializeField] private DeathKillNode[] alternateNodes; 

    public DeathKillNode GetAlternateNode()
    {
        return alternateNodes[Random.Range(0, alternateNodes.Length)];
    }

}
