using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainNode : OverworldNode
{
    [SerializeField] Transform boardingTransformTrainStation;
    [SerializeField] Transform boardingTransformKlipsolsCove;

    bool trackPlayerPosition;
    bool board;
    bool trackPositiveX;
    OverworldCharacter playerController;
    Transform boardingTransform;
    Transform[] trainPositionsToTraverse;
    OwTrain train;

    private void Start()
    {
        playerController = transform.parent.parent.Find("OW_ControllerCharacter").GetComponent<OverworldCharacter>();
        train = transform.parent.parent.Find("Train").GetComponent<OwTrain>();
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
        trainPositionsToTraverse = LeftNode.UpTransforms;
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
        trainPositionsToTraverse = DownNode.DownTransforms;
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
    }
}
