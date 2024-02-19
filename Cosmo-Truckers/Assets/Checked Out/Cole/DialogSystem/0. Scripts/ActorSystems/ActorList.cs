using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorList : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerActorPrefabs;
    [SerializeField] private List<GameObject> additionalActorPrefabsInLayout; 
    public List<GameObject> GetPlayerActorPrefabs() { return playerActorPrefabs; }
    public List<GameObject> GetAdditionalActorPrefabs() { return additionalActorPrefabsInLayout; }
}
