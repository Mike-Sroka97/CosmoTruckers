using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCombatNode : MonoBehaviour
{
    public bool Boss;
    public bool CombatDone;
    public string SceneToLoad;

    [SerializeField] List<GameObject> enemiesToSpawn;

    public IEnumerator StartCombat(DNode currentNode)
    {
        DungeonController controller = FindObjectOfType<DungeonController>();

        //Start the flippy floppy as long as it is in scene
        FlipLoadAnimation flip = FindObjectOfType<FlipLoadAnimation>();
        if (flip)
        {
            flip.InitFlip();
            yield return new WaitUntil(() => !flip.IsFlipping);
        }

        //Set dungeon refs
        CombatManager.Instance.LastCameraPosition = CameraController.Instance.transform.position;
        CombatManager.Instance.DungeonCharacterInstance = CameraController.Instance.Leader;
        CombatManager.Instance.CurrentNode = currentNode;
        CombatManager.Instance.InCombat = true;

        //Setup combat refs
        CameraController.Instance.Leader = null;
        CameraController.Instance.transform.position = new Vector3(controller.CombatCameraPosition.position.x, controller.CombatCameraPosition.position.y, CameraController.Instance.transform.position.z);

        //Start combat
        currentNode.Active = false;
        EnemyManager.Instance.EnemiesToSpawn = enemiesToSpawn;
        EnemyManager.Instance.InitializeEnemys();
    }
}
