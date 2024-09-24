using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCharacter : MonoBehaviour
{
    [SerializeField] Sprite[] characterControllerSprites;
    [SerializeField] protected float moveSpeed;

    protected DungeonController dungeon;
    protected bool moving;
    SpriteRenderer myRenderer;
    bool startup = true;

    /// <summary>
    /// Probably want Overworld.cs to handle the initial node at some point
    /// </summary>
    protected virtual void Start()
    {
        myRenderer = GetComponentInChildren<SpriteRenderer>();
        dungeon = FindObjectOfType<DungeonController>();
        transform.position = dungeon.PlayerStartPosition.position;

        enabled = false;

        CameraController.Instance.InitializeOwCamera(dungeon.minCameraX, dungeon.maxCameraX, dungeon.minCameraY, dungeon.maxCameraY, transform);
        CameraController.Instance.StartCoroutine(CameraController.Instance.FadeVignette(true));

        myRenderer.sprite = characterControllerSprites[CombatData.Instance.PlayersToSpawn[0].PlayerID];

        StartCoroutine(MoveToStart());
    }

    private void Update()
    {
        TrackInput();
    }

    private void TrackInput()
    {
        if (moving || startup)
            return;

        //Interact
        if (Input.GetKeyDown(KeyCode.Space) && dungeon.CurrentNode.Active)
            dungeon.CurrentNode.Interact();

        if (dungeon.CurrentNode.NodeData.GetComponent<DungeonCombatNode>() && !dungeon.CurrentNode.NodeData.GetComponent<DungeonCombatNode>().CombatDone)
            return;

        //flip sprite
        if (Input.GetKeyDown(KeyCode.A) && myRenderer)
            myRenderer.flipX = true;

        else if (Input.GetKeyDown(KeyCode.D) && myRenderer)
            myRenderer.flipX = false;

        //Left
        if (Input.GetKeyDown(KeyCode.A))
            dungeon.CurrentNode.SelectNode(true);

        //Right
        else if (Input.GetKeyDown(KeyCode.D))
            dungeon.CurrentNode.SelectNode(false);
    }

    public virtual IEnumerator Move(DNode newCurrentNode, Transform[] pointsToTraverse)
    {
        dungeon.CurrentNode = newCurrentNode;

        moving = true;
        int currentPoint = 0;

        while (currentPoint < pointsToTraverse.Length)
        {
            //Move toward each point one by one
            while (transform.position != pointsToTraverse[currentPoint].transform.position)
            {
                float lastX = transform.position.x;

                transform.position = Vector3.MoveTowards(transform.position, pointsToTraverse[currentPoint].transform.position, moveSpeed * Time.deltaTime);

                //Flip on complex up / down movements
                if (lastX > transform.position.x)
                    myRenderer.flipX = true;
                else if (lastX < transform.position.x)
                    myRenderer.flipX = false;

                //Wait a frame you fucking nerd
                yield return null;
            }
            currentPoint++;
        }

        dungeon.CurrentNode.SetupNode();

        moving = false;
    }

    private IEnumerator MoveToStart()
    {
        while (CameraController.Instance.CurrentlyExecutingCommand)
            yield return null;

        while (transform.position != dungeon.CurrentNode.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, dungeon.CurrentNode.transform.position, Time.deltaTime * moveSpeed / 2);
            yield return null;
        }

        startup = false;
        dungeon.CurrentNode.SetupNode();
    }
}
