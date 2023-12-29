using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatData : MonoBehaviour
{
    public static CombatData Instance;
    public GameObject EnemysToSpawn = null;
    public List<GameObject> PlayersToSpawn = new();

    private void OnEnable()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

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
                Destroy(this.gameObject);
                CombatData.Instance = null;
            }
        }
    }
}
