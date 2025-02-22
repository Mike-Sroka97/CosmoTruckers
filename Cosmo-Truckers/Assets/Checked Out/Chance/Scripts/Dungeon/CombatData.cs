using System.Collections.Generic;
using UnityEngine;

public class CombatData : MonoBehaviour
{
    public bool TESTING = true;

    public static CombatData Instance;
    public List<GameObject> PlayersToSpawn = new();

    private void Awake()
    {
        CombatData[] objs = FindObjectsOfType<CombatData>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (!Instance)
            Instance = this;
    }
}
