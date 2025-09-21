using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCombatNode : MonoBehaviour
{
    public bool Boss;
    public string SceneToLoad;

    [SerializeField] List<GameObject> enemiesToSpawn;
    [SerializeField] bool playMusicOnCombatStart = true;
    private SceneMusic combatMusic; 

    public IEnumerator StartCombat(DNode currentNode)
    {
        // Grab the combat music
        combatMusic = GetComponent<SceneMusic>();

        //Set dungeon refs
        CombatManager.Instance.LastCameraPosition = CameraController.Instance.transform.position;
        CombatManager.Instance.DungeonCharacterInstance = CameraController.Instance.Leader;
        CombatManager.Instance.CurrentNode = currentNode;
        CombatManager.Instance.InCombat = true;
        CameraController.Instance.Leader = null;
        currentNode.Active = false;

        // Set Enemies
        EnemyManager.Instance.EnemiesToSpawn = enemiesToSpawn;
        EnemyManager.Instance.InitializeEnemys();

        // Start the flippy floppy as long as it is in scene
        FlipLoadAnimation flip = FindObjectOfType<FlipLoadAnimation>();
        if (flip)
        {
            flip.InitFlip();
            yield return new WaitUntil(() => !flip.IsFlipping);
        }

        // Start 
        TurnOrder.Instance.StartTurnOrder();

        // Play the music when combat starts, unless music will be played later for cinematic effect
        if (playMusicOnCombatStart)
            combatMusic.PlaySceneTrack(); 
    }
}
