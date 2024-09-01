using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovasLandingOverworld : Overworld
{
    [Header("First Time Visit Stuff")]
    [SerializeField] GameObject olaris;
    [SerializeField] Transform[] olarisPointsToMove;
    [SerializeField] float olarisFadeSpeed;
    [SerializeField] OverworldNode olarisHomeNode;
    [SerializeField] float olarisMoveSpeed;
    bool firstTimeSetup = true; 

    [Space(20)]
    [Header("Yed Stuff")]
    [SerializeField] OverworldNode yedNode;
    [SerializeField] OverworldNode yedNodeToEnable;
    [SerializeField] OverworldNode d1Node;

    [Space(20)]
    [Header("Dungeon One Stuff")]

    [Space(20)]
    [Header("Post-dungeon one")]
    [SerializeField] OverworldNode loonaNode;

    [Space(20)]
    [Header("Klipsol Rock Stuff")]
    [SerializeField] Transform klipsolRock;
    [SerializeField] Transform klipsolRockDesination;
    [SerializeField] float klipsolRockMoveSpeed;
    [SerializeField] SpriteRenderer klipsolRockReceiver;
    [SerializeField] Sprite klipsolRockInserted;
    [SerializeField] OverworldNode d2Node;
    [SerializeField] OverworldNode rockPlacingNode;

    [Space(20)]
    [Header("Post-dungeon two")]
    [SerializeField] OverworldNode kleptorNode;
    [SerializeField] OverworldNode d3Node;

    [Space(20)]
    [Header("Post Kleptor cutscene")]
    [SerializeField] OverworldNode smallDogNode;
    [SerializeField] OverworldNode smallDogDestinationNode;

    [Space(20)]
    [Header("Post Kleptor Loona")]
    [SerializeField] OverworldNode kleptorLoonaNode;
    [SerializeField] OverworldNode noseNode;
    [SerializeField] OverworldNode d4Node;

    DimensionOneLevelData data;

    protected override void OverworldInitialize()
    {
        if(data == null)
            data = SaveManager.LoadDimensionOne();

        StartCoroutine(WaitAFrame());

        SetupStartingNode(); 

        //Pre cutscene
        /*
         * Olaris making back-shots at his daughters front yard. Despondantly returns home *map event*
         * Any node past train is disabled
         * Olaris's home is the only active node in this state 
         */
        if (!data.PreludeOlarisTalkedTo)
        {
            PreludeOlarisMapEvent();
        }

        else if (!data.PreludeYedTalkedTo)
        {
            PreludeYed();
        }

        else if(data.PreludeYedTalkedTo && !data.DungeonsCompleted[0])
        {
            d1Node.ActivateNode();
        }

        //Post dungeon 1
        /*
         * Loona makes a snail trail home *map event*
         * Loona house node is enabled *cutscene*
         * Loona grants rock for KC node *map event*
         */

        else if (!data.LoonaTalkedToPostDungeonOne)
        {
            loonaNode.ActivateNode();
        }

        else if (!data.MoonStonePlaced)
        {
            rockPlacingNode.ActivateNode();
        }

        else if (!data.MoonStonePlaced && !data.DungeonsCompleted[1])
        {
            d2Node.ActivateNode();
        }

        //Post dungeon 2
        /*
         * Chlamydia gives the party a bag in dungeon cutscene??
         * Party brings bag to Klepto's house *cutscene*
         * Interactive node where dog coughs out second magical bag where long dog pulls out small dog that stretches across to D3 *map event*
         */

        else if (!data.KleptorTalkedToPostDungeonTwo)
        {
            kleptorNode.ActivateNode();
        }

        else if (!data.SmallDogStretched)
        {
            smallDogNode.ActivateNode();
        }

        else if(!data.DungeonsCompleted[2])
        {
            d3Node.ActivateNode();
        }

        //Post dungeon 3
        /*
         * Small dog is now a skeleton because he died
         * Orbnus explodes => schnoze moves into place making d4 accessable *map event*
         * Loon-dog :penguin-dance: south of ldg node SW of D3. Unlocks D4 *cutscene*
         */

        else if (!data.LoonaTalkedToPostDungeonThree && data.DungeonsCompleted[2])
        {
            //do nose stuff??
            kleptorLoonaNode.ActivateNode();
        }

        else if (data.LoonaTalkedToPostDungeonThree && !data.DungeonsCompleted[3])
        {
            d4Node.ActivateNode();
        }

        //Post dungeon 4
        /*
         * Party map event at kleptor *map event*
         * optional cutscene at kleptor. Rink dinkus grants you a legendary wad of cum
         * Dungeons replayable
         * Bounty board????????
         */

        else if(!data.AfterPartyAttended)
        {
            //TODO get fucked Cole
        }

        else
        {
            //TODO continued to get fucked Cole
        }

        SetupStartNodes();
    }

    /// <summary>
    /// Set the node that the player actually spawns onto and 
    /// </summary>
    protected override void SetupStartingNode()
    {
        // Only call this portion when loading into the scene
        if (firstTimeSetup)
        {
            firstTimeSetup = false;

            // Get the previous node and set the current node to it
            OverworldNode startNode = GameObject.Find(data.PreviousNode).GetComponent<OverworldNode>();
            CurrentNode = startNode != null ? startNode : CurrentNode;

            // Set the player to the position of the previous node
            OverworldCharacter mapPlayer = GameObject.Find("OW_ControllerCharacter").GetComponent<OverworldCharacter>();
            mapPlayer.transform.position = CurrentNode.transform.position;
        }

        // Setup the current node 
        CurrentNode.SetupNode();
    }

    private void SetupStartNodes()
    {
        if(data.PreludeYedTalkedTo)
        {
            yedNodeToEnable.Active = true;
            yedNodeToEnable.DetermineState();
        }

        if(data.MoonStonePlaced)
        {
            klipsolRock.position = klipsolRockDesination.position;
            d2Node.Active = true;
            d2Node.DetermineState();
        }

        if(data.SmallDogStretched)
        {
            //kill the dog LOL
            smallDogDestinationNode.Active = true;
            smallDogDestinationNode.DetermineState();
        }

        if(data.DungeonsCompleted[2])
        {
            noseNode.Active = true;
            noseNode.DetermineState();
        }

        if(data.LoonaTalkedToPostDungeonThree)
        {
            d4Node.Active = true;
            d4Node.DetermineState();
        }

        if(!data.AfterPartyAttended)
        {
            //Activate party node. Might need to override selection event
        }
    }

    public void DebugDungeonComplete(int dungeonNumber)
    {
        data.DungeonsCompleted[dungeonNumber] = true;
        data.SaveLevelData();
        OverworldInitialize();

        //funny haha clean up
        d1Node.LookAtMeSprite.SetActive(false);
        d2Node.LookAtMeSprite.SetActive(false);
        d3Node.LookAtMeSprite.SetActive(false);
        d4Node.LookAtMeSprite.SetActive(false);
    }

    private void Update()
    {
        if (debugging)
            DebugInput();
    }

    protected override void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
            data.DeleteLevelData();
    }

    private void PreludeOlarisMapEvent()
    {
        enableLeaderOnFade = false;
        StartCoroutine(FadeOlaris(olaris.GetComponent<SpriteRenderer>(), true));
    }

    IEnumerator WaitAFrame()
    {
        yield return null;
    }

    IEnumerator FadeOlaris(SpriteRenderer renderer, bool fadeIn)
    {
        if(fadeIn)
        {
            yield return new WaitForSeconds(1f);

            while (renderer.color.a < 1)
            {
                renderer.color += new Color(0, 0, 0, olarisFadeSpeed * Time.deltaTime);
                yield return null;
            }

            StartCoroutine(MoveOlaris());
        }
        else
        {
            while (renderer.color.a > 0)
            {
                renderer.color -= new Color(0, 0, 0, olarisFadeSpeed * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            CameraController.Instance.Leader.GetComponent<OverworldCharacter>().enabled = true;
            olarisHomeNode.ActivateNode();
        }
    }

    IEnumerator MoveOlaris()
    {
        while (CameraController.Instance.CommandsExecuting > 0)
            yield return null;

        for (int i = 0; i < olarisPointsToMove.Length; i++)
        {
            if (i == 1)
                olaris.GetComponent<SpriteRenderer>().flipX = true;

            Vector3 currentGoal = new Vector3(olarisPointsToMove[i].position.x, olarisPointsToMove[i].position.y + 0.5f, olarisPointsToMove[i].position.z);

            while (olaris.transform.position != currentGoal)
            {
                olaris.transform.position = Vector3.MoveTowards(olaris.transform.position, currentGoal, olarisMoveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        StartCoroutine(FadeOlaris(olaris.GetComponent<SpriteRenderer>(), false));
    }

    public void SaveOlarisPrelude(OverworldNode node)
    {
        olarisHomeNode.DeactiveNode();
        data.PreludeOlarisTalkedTo = true;
        data.PreviousNode = node.name;
        data.SaveLevelData();
        OverworldInitialize();
    }

    public void SaveLoonaDungeonOne()
    {
        loonaNode.DeactiveNode();
        data.LoonaTalkedToPostDungeonOne = true;
        data.SaveLevelData();
        OverworldInitialize();
    }

    public void SaveKleptorDungeonTwo()
    {
        kleptorNode.DeactiveNode();
        data.KleptorTalkedToPostDungeonTwo = true;
        data.SaveLevelData();
        OverworldInitialize();
    }

    public void SaveLoonaKleptorDungeonThree()
    {
        kleptorLoonaNode.DeactiveNode();
        data.LoonaTalkedToPostDungeonThree = true;
        data.SaveLevelData();
        OverworldInitialize();
    }

    public void SaveSmallDog()
    {
        smallDogDestinationNode.Active = true;
        smallDogNode.DetermineState();
        smallDogNode.DeactiveNode();
        data.SmallDogStretched = true;
        data.SaveLevelData();
        OverworldInitialize();
    }

    private void PreludeYed()
    {
        yedNode.ActivateNode();
    }

    public void TalkToYed()
    {
        yedNodeToEnable.Active = true;
        yedNodeToEnable.DetermineState();
        yedNode.DeactiveNode();

        data.PreludeYedTalkedTo = true;
        data.SaveLevelData();
        OverworldInitialize();
    }

    public void MoveKlippsolRock()
    {
        StartCoroutine(KlippsolRockCoroutine());
    }

    IEnumerator KlippsolRockCoroutine()
    {
        CurrentNode.DeactiveNode();
        klipsolRockReceiver.sprite = klipsolRockInserted;

        OverworldCharacter character = FindObjectOfType<OverworldCharacter>();
        character.enabled = false;

        while(klipsolRock.position != klipsolRockDesination.position)
        {
            klipsolRock.position = Vector3.MoveTowards(klipsolRock.position, klipsolRockDesination.position, klipsolRockMoveSpeed * Time.deltaTime);
            yield return null;
        }

        d2Node.Active = true;
        d2Node.ActivateNode();
        CurrentNode.SetupNode();

        character.enabled = true;

        //save data
        data.MoonStonePlaced = true;
        SaveManager.SaveDimensionOne(data);
    }
}
