using UnityEngine;
using UnityEngine.Events;


public class OverworldNode : MonoBehaviour
{
    //Sprites for node
    [SerializeField] Sprite activeInteractiveNode;
    [SerializeField] Sprite deactiveInteractiveNode;
    [SerializeField] Sprite activeNode;
    [SerializeField] Sprite deactiveNode;
    [SerializeField] Sprite activeInteractiveUsedNode;

    public UnityEvent MoveUpEvent;
    public UnityEvent MoveLeftEvent;
    public UnityEvent MoveDownEvent;
    public UnityEvent MoveRightEvent;

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
    public GameObject LookAtMeSprite;

    [SerializeField] string sceneToLoad;
    public UnityEvent MapEventInteraction;

    protected SpriteRenderer myRenderer;

    /// <summary>
    /// The second layer of Overworld music to play
    /// </summary>
    [SerializeField] SceneMusic AdditionalOverworldMusic;

    /// <summary>
    /// Whether to start (true) or stop (false) additional overworld music
    /// </summary>
    [SerializeField] bool owMusicState;

    protected virtual void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        DetermineState();
    }

    private void OnDestroy()
    {
        MoveUpEvent.RemoveAllListeners();
        MoveLeftEvent.RemoveAllListeners();
        MoveDownEvent.RemoveAllListeners();
        MoveRightEvent.RemoveAllListeners();
        MapEventInteraction.RemoveAllListeners();
    }

    public void DetermineState()
    {
        if(!myRenderer)
            myRenderer = GetComponent<SpriteRenderer>();

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
        if (sceneToLoad != "")
        {
            foreach(OverworldCharacter character in FindObjectsOfType<OverworldCharacter>())
                character.enabled = false;

            FindObjectOfType<Overworld>().LoadingScene = true;
            CameraController.Instance.LastNode = gameObject.name;
            StartCoroutine(CameraController.Instance.OwCharacterActionSelect(sceneToLoad, MapEventInteraction));
        }
        else { MapEventInteraction?.Invoke();}
    }

    public void ActivateNode()
    {
        Interactive = true;
        LookAtMeSprite.SetActive(true);
        DetermineState();
    }

    public void DeactivateNode()
    {
        Interactive = false;
        LookAtMeSprite.SetActive(false);
        DetermineState();
    }

    protected virtual void SetupLineRendererers()
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

    protected void SetLinePositions(Transform[] points, LineRenderer currentLine, OverworldNode node)
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
        if(!myRenderer)
            myRenderer = GetComponent<SpriteRenderer>();

        if (Active && Interactive)
            myRenderer.sprite = activeInteractiveUsedNode;

        SetupLineRendererers();

        // Determine if the Additional Overworld Music exists, and if it should be played or stopped
        if (AdditionalOverworldMusic != null)
        {
            if (owMusicState)
                AudioManager.Instance.PlayAlternateTrack(AdditionalOverworldMusic);
            else
                AudioManager.Instance.StopAlternateTrack(AdditionalOverworldMusic.FadeDuration); 
        }
    }

    public void LeavingNodeCleanup()
    {
        if (Active && Interactive)
            myRenderer.sprite = activeInteractiveNode;

        CleanupLineRenderers();
    }
}
