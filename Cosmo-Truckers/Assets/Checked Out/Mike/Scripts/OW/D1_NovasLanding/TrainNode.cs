using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainNode : OverworldNode
{
    [SerializeField] Transform boardingTransformTrainStation;
    [SerializeField] Transform boardingTransformKlipsolsCove;

    //Start "fuck Node" (7) sector
    [SerializeField] bool overrideLineDraws;
    [SerializeField] Transform[] trainDownTransforms;
    [SerializeField] Transform[] hallDownTransforms;
    //End "fuck Node (7)" sector

    bool trackPlayerPosition;
    bool board;
    bool trackPositiveX;
    OverworldCharacter playerController;
    Transform boardingTransform;
    Transform[] trainPositionsToTraverse;
    OwTrain train;

    protected override void Start()
    {
        playerController = transform.parent.parent.Find("OW_ControllerCharacter").GetComponent<OverworldCharacter>();
        train = transform.parent.parent.Find("Train").GetComponent<OwTrain>();
        base.Start();
    }

    private void Update()
    {
        //Don't do shit unless you are tracking
        if (!trackPlayerPosition)
            return;

        //Do train stuff if the player position meets prereq's
        if(trackPositiveX && playerController.transform.position.x > boardingTransform.position.x)
            trackPlayerPosition = false;

        else if(!trackPositiveX && playerController.transform.position.x < boardingTransform.position.x)
            trackPlayerPosition = false;

        //Boarding process
        if (!trackPlayerPosition)
        {
            if (board)
                Board();
            else
                Unboard();
        }
    }

    public void GetOnTrainFromTrainStation()
    {
        //Node right of train station
        trainPositionsToTraverse = RightNode.RightTransforms;
        InitTrain(true, true, boardingTransformTrainStation);
    }

    public void GetOnTrainFromKlipsolsCove()
    {
        //Node left/up from klipsol's tube
        trainPositionsToTraverse = LeftNode.LeftTransforms;
        InitTrain(true, false, boardingTransformKlipsolsCove);
    }

    public void GetOffTrainFromTrainStation()
    {
        //Node left of Train Station
        trainPositionsToTraverse = LeftNode.RightTransforms;
        InitTrain(false, false, boardingTransformTrainStation);
    }

    public void GetOffTrainFromKlipsolsCove()
    {
        //Node right of klipsol's tube
        trainPositionsToTraverse = DownNode.LeftTransforms;
        InitTrain(false, true, boardingTransformKlipsolsCove);
    }

    private void InitTrain(bool boarding, bool positionCheck, Transform currentBoardingTransform)
    {
        trackPlayerPosition = true;
        board = boarding;
        trackPositiveX = positionCheck;
        boardingTransform = currentBoardingTransform;
    }

    private void Board()
    {
        Debug.Log($"Now boarding: {boardingTransform.name}");
        train.Board(boardingTransform, playerController, trainPositionsToTraverse);
    }

    private void Unboard()
    {
        Debug.Log($"Now unboarding: {boardingTransform.name}");
        train.Unboard(boardingTransform, playerController, trainPositionsToTraverse);
    }

    //I like to call this block of the code the "fuck Node (7)" section. Node (7)'s entire family could burn in a fire for all I care
    public void SetDownTransformsFive()
    {
        DownTransforms = trainDownTransforms;
    }

    public void SetDownTransformsTen()
    {
        DownTransforms = hallDownTransforms;
    }

    protected override void SetupLineRendererers()
    {
        if(!overrideLineDraws)
        {
            base.SetupLineRendererers();
        }
        else
        {
            LineRenderer currentLine;

            //Up line
            currentLine = transform.Find("LineRendererUp").GetComponent<LineRenderer>();
            SetLinePositions(hallDownTransforms, currentLine, UpNode);

            //Left line
            currentLine = transform.Find("LineRendererLeft").GetComponent<LineRenderer>();
            SetLinePositions(trainDownTransforms, currentLine, LeftNode);

            //Right line
            currentLine = transform.Find("LineRendererRight").GetComponent<LineRenderer>();
            SetLinePositions(LeftTransforms, currentLine, RightNode);
        }
    }
    //End "fuck Node (7)"
}
