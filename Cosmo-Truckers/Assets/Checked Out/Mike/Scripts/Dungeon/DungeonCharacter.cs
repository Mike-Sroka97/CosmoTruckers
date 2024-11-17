using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCharacter : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;

    protected DungeonController dungeon;
    protected bool moving;
    SpriteRenderer myRenderer;
    bool startup = true;
    Animator myAnimator;

    const string idle = "Idle";
    const string move = "Move";
    const string win = "Win";
    const string death = "Death";

    /// <summary>
    /// Probably want Overworld.cs to handle the initial node at some point
    /// </summary>
    protected virtual void Start()
    {
        myRenderer = GetComponentInChildren<SpriteRenderer>();
        dungeon = FindObjectOfType<DungeonController>();
        myAnimator = GetComponentInChildren<Animator>();
        myAnimator.Play(move);
        transform.position = dungeon.PlayerStartPosition.position;

        enabled = false;

        CameraController.Instance.InitializeOwCamera(dungeon.minCameraX, dungeon.maxCameraX, dungeon.minCameraY, dungeon.maxCameraY, transform);
        CameraController.Instance.StartCoroutine(CameraController.Instance.FadeVignette(true));

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

        if (dungeon.CurrentNode.NodeData.GetComponent<DungeonCombatNode>() && !dungeon.CurrentNode.CombatDone)
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

    public virtual IEnumerator Move(DNode newCurrentNode, List<Vector3> positions)
    {
        List<Vector3> tempPositions = new List<Vector3>();
        foreach (Vector3 vertex in positions)
            tempPositions.Add(vertex);

        dungeon.CurrentNode = newCurrentNode;

        moving = true;
        myAnimator.Play(move);
        int currentPoint = 0;

        while (currentPoint < tempPositions.Count)
        {
            //Move toward each point one by one
            while (transform.position != tempPositions[currentPoint])
            {
                float lastX = transform.position.x;

                transform.position = Vector3.MoveTowards(transform.position, tempPositions[currentPoint], moveSpeed * Time.deltaTime);

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

        myAnimator.Play(idle);
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
        myAnimator.Play(idle);
        dungeon.CurrentNode.SetupNode();
    }

    public void SetAnimator(string animation)
    {
        myAnimator.Play(animation);
    }
}
