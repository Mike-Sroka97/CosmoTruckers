using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatData : MonoBehaviour
{
    public static CombatData Instance;
    public Vector2 combatLocation = new Vector2(0, 0);
    public int dungeonSeed = 0;
    public GameObject EnemysToSpawn = null;
    public List<GameObject> PlayersToSpawn = new();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += ChangeScene;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= ChangeScene;
    }

    private void ChangeScene(Scene current, Scene next)
    {
        if (next.name != "Combat Test")
        {
            if (next.name != "DungeonSelection")
            {
                CombatData.Instance = null;
                Destroy(this.gameObject);
            }
        }
    }
}
