using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCharacter : MonoBehaviour
{
    [SerializeField] protected float moveSpeed;

    protected Overworld overworld;
    protected bool moving;
    SpriteRenderer myRenderer;

    /// <summary>
    /// Probably want Overworld.cs to handle the initial node at some point
    /// </summary>
    protected virtual void Start()
    {
        myRenderer = GetComponentInChildren<SpriteRenderer>();
        overworld = FindObjectOfType<Overworld>();
        transform.position = overworld.CurrentNode.transform.position;

        enabled = false;

        CameraController.Instance.InitializeOwCamera(overworld.minCameraX, overworld.maxCameraX, overworld.minCameraY, overworld.maxCameraY, transform);
        CameraController.Instance.StartCoroutine(CameraController.Instance.FadeVignette(true));
        overworld.CurrentNode.SetupNode();
    }

    private void Update()
    {
        TrackInput();
    }

    private void TrackInput()
    {
        if (moving)
            return;

        //flip sprite
        if (Input.GetKeyDown(KeyCode.A) && myRenderer)
            myRenderer.flipX = true;

        else if (Input.GetKeyDown(KeyCode.D) && myRenderer)
            myRenderer.flipX = false;

        //Interact
        if (Input.GetKeyDown(KeyCode.Space) && overworld.CurrentNode.Interactive && overworld.CurrentNode.Active)
            overworld.CurrentNode.Interact(); 

        //Up
        else if (Input.GetKeyDown(KeyCode.W) && overworld.CurrentNode.UpNode && overworld.CurrentNode.UpNode.Active)
        {
            overworld.CurrentNode.MoveUpEvent?.Invoke();
            overworld.CurrentNode.LeavingNodeCleanup();
            overworld.CurrentNode = overworld.CurrentNode.UpNode;
            StartCoroutine(Move(overworld.CurrentNode.UpTransforms));
        }

        //Left
        else if (Input.GetKeyDown(KeyCode.A) && overworld.CurrentNode.LeftNode && overworld.CurrentNode.LeftNode.Active)
        {
            overworld.CurrentNode.MoveLeftEvent?.Invoke();
            overworld.CurrentNode.LeavingNodeCleanup();
            overworld.CurrentNode = overworld.CurrentNode.LeftNode;
            StartCoroutine(Move(overworld.CurrentNode.LeftTransforms));
        }

        //Down
        else if (Input.GetKeyDown(KeyCode.S) && overworld.CurrentNode.DownNode && overworld.CurrentNode.DownNode.Active)
        {
            overworld.CurrentNode.MoveDownEvent?.Invoke();
            overworld.CurrentNode.LeavingNodeCleanup();
            overworld.CurrentNode = overworld.CurrentNode.DownNode;
            StartCoroutine(Move(overworld.CurrentNode.DownTransforms));
        }

        //Right
        else if (Input.GetKeyDown(KeyCode.D) && overworld.CurrentNode.RightNode && overworld.CurrentNode.RightNode.Active)
        {
            overworld.CurrentNode.MoveRightEvent?.Invoke();
            overworld.CurrentNode.LeavingNodeCleanup();
            overworld.CurrentNode = overworld.CurrentNode.RightNode;
            StartCoroutine(Move(overworld.CurrentNode.RightTransforms));
        }
    }

    public virtual IEnumerator Move(Transform[] pointsToTraverse)
    {
        moving = true;
        int currentPoint = 0;

        while(currentPoint < pointsToTraverse.Length)
        {
            //Move toward each point one by one
            while(transform.position != pointsToTraverse[currentPoint].transform.position)
            {
                float lastX = transform.position.x;

                transform.position = Vector3.MoveTowards(transform.position, pointsToTraverse[currentPoint].transform.position, moveSpeed * Time.deltaTime);

                //Flip on complex up / down movements
                if (lastX > transform.position.x)
                    myRenderer.flipX = true;
                else if(lastX < transform.position.x)
                    myRenderer.flipX = false;

                //Wait a frame you fucking nerd
                yield return null;
            }
            currentPoint++;
        }

        overworld.CurrentNode.SetupNode();

        moving = false;
    }
}
