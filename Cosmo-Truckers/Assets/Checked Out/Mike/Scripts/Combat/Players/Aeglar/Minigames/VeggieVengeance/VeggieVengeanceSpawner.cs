using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class VeggieVengeanceSpawner : MonoBehaviour
{
    List<VeggieVengeanceSpawnPoint> spawnPoints;

    public void Initialize()
    {
        spawnPoints = new List<VeggieVengeanceSpawnPoint>();
        spawnPoints = FindObjectsOfType<VeggieVengeanceSpawnPoint>().ToList(); 
    }

    /// <summary>
    /// Get the location to spawn this veggie and then remove it so another does not spawn in the same location
    /// </summary>
    /// <returns>Veggie spawn point</returns>
    public VeggieVengeanceSpawnPoint GetSpawnPoint()
    {
        VeggieVengeanceSpawnPoint randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        spawnPoints.Remove(randomSpawnPoint);

        return randomSpawnPoint;
    }

}
