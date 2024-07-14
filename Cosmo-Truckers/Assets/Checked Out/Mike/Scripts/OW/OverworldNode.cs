using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldNode : MonoBehaviour
{
    //Sprites for node
    [SerializeField] Sprite activeInteractiveNode;
    [SerializeField] Sprite deactiveInteractiveNode;
    [SerializeField] Sprite activeNode;
    [SerializeField] Sprite deactiveNode;
    [SerializeField] Sprite activeInteractiveUsedNode;

    //Nodes that the player can move to
    public OverworldNode UpNode;
    public OverworldNode LeftNode;
    public OverworldNode DownNode;
    public OverworldNode RightNode;

    //Interactivity of the node
    public bool Interactive = true;
    public bool Active = true;

    //Transforms for player to traverse per direction
    public Transform[] UpTransforms;
    public Transform[] LeftTransforms;
    public Transform[] DownTransforms;
    public Transform[] RightTransforms;

    [SerializeField] string sceneToLoad;

    SpriteRenderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();

        //TODO call from dimension OW script
        DetermineState();
    }

    public void DetermineState()
    {
        if (Active && Interactive && FindObjectOfType<Overworld>().CurrentNode == this)
        {
            myRenderer.sprite = activeInteractiveUsedNode;
            SetupLineRendererers();
        }

        else if (Active && Interactive)
            myRenderer.sprite = activeInteractiveNode;

        else if (!Active && Interactive)
            myRenderer.sprite = deactiveInteractiveNode;

        else if (Active && !Interactive)
            myRenderer.sprite = activeNode;

        else
            myRenderer.sprite = deactiveNode;
    }

    public void Interact()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void SetupLineRendererers()
    {
        LineRenderer currentLine;

        //Up line
        currentLine = transform.Find("LineRendererUp").GetComponent<LineRenderer>();

        if(UpNode && UpNode.Active)
        {
            if(UpNode.transform.position.x < transform.position.x)
                SetLinePositions(RightTransforms, currentLine, UpNode);
            else if(UpNode.transform.position.x > transform.position.x)
                SetLinePositions(LeftTransforms, currentLine, UpNode);
            else
                SetLinePositions(DownTransforms, currentLine, UpNode);
        }

        //Left line
        currentLine = transform.Find("LineRendererLeft").GetComponent<LineRenderer>();

        if (LeftNode && LeftNode.Active)
        {
            if (LeftNode.transform.position.y < transform.position.y)
                SetLinePositions(UpTransforms, currentLine, LeftNode);
            else if (LeftNode.transform.position.y > transform.position.y)
                SetLinePositions(DownTransforms, currentLine, LeftNode);
            else
                SetLinePositions(RightTransforms, currentLine, LeftNode);
        }

        //Down line
        currentLine = transform.Find("LineRendererDown").GetComponent<LineRenderer>();

        if (DownNode && DownNode.Active)
        {
            if (DownNode.transform.position.x < transform.position.x)
                SetLinePositions(RightTransforms, currentLine, DownNode);
            else if (DownNode.transform.position.x > transform.position.x)
                SetLinePositions(LeftTransforms, currentLine, DownNode);
            else
                SetLinePositions(UpTransforms, currentLine, DownNode);
        }

        //Right line
        currentLine = transform.Find("LineRendererRight").GetComponent<LineRenderer>();

        if (RightNode && RightNode.Active)
        {
            if (RightNode.transform.position.y < transform.position.y)
                SetLinePositions(DownTransforms, currentLine, RightNode);
            else if (RightNode.transform.position.y > transform.position.y)
                SetLinePositions(UpTransforms, currentLine, RightNode);
            else
                SetLinePositions(LeftTransforms, currentLine, RightNode);
        }
    }

    private void SetLinePositions(Transform[] points, LineRenderer currentLine, OverworldNode node)
    {
        currentLine.positionCount = points.Length + 1;

        int i = 0;

        currentLine.SetPosition(i, node.transform.position);
        i++;

        foreach (Transform nodePoint in points)
        {
            currentLine.SetPosition(i, nodePoint.position);
            i++;
        }
    }

    private void CleanupLineRenderers()
    {
        LineRenderer currentLine;

        //Up line
        currentLine = transform.Find("LineRendererUp").GetComponent<LineRenderer>();
        currentLine.positionCount = 0;

        //Left line
        currentLine = transform.Find("LineRendererLeft").GetComponent<LineRenderer>();
        currentLine.positionCount = 0;

        //Down line
        currentLine = transform.Find("LineRendererDown").GetComponent<LineRenderer>();
        currentLine.positionCount = 0;

        //Right line
        currentLine = transform.Find("LineRendererRight").GetComponent<LineRenderer>();
        currentLine.positionCount = 0;
    }

    //handles line renderers and active State
    public void SetupNode()
    {
        if(Active && Interactive)
            myRenderer.sprite = activeInteractiveUsedNode;

        SetupLineRendererers();
    }

    public void LeavingNodeCleanup()
    {
        if (Active && Interactive)
            myRenderer.sprite = activeInteractiveNode;

        CleanupLineRenderers();
    }
}
