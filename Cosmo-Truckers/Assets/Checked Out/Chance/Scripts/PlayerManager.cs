using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public List<CharacterSO> AllCharacters;

    /// <summary>
    /// Sets non destroyable manager
    /// </summary>
    private void Awake()
    {
        PlayerManager[] objs = FindObjectsOfType<PlayerManager>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (!Instance)
            Instance = this;
    }
}
