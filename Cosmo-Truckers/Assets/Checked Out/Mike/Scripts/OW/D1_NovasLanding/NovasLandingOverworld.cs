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

    [Space(20)]
    [Header("Yed Stuff")]
    [SerializeField] OverworldNode yedNode;
    [SerializeField] OverworldNode yedNodeToEnable;
    [SerializeField] OverworldNode dungeonOneNode;

    [Space(20)]
    [Header("Dungeon One Stuff")]

    [Space(20)]
    [Header("Klipsol Rock Stuff")]
    [SerializeField] Transform klipsolRock;
    [SerializeField] Transform klipsolRockDesination;
    [SerializeField] float klipsolRockMoveSpeed;
    [SerializeField] SpriteRenderer klipsolRockReceiver;
    [SerializeField] Sprite klipsolRockInserted;
    [SerializeField] OverworldNode d3Node;

    DimensionOneLevelData data;

    protected override void OverworldInitialize()
    {
        if(data == null)
            data = SaveManager.LoadDimensionOne();

        StartCoroutine(WaitAFrame());

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

        else if(data.PreludeYedTalkedTo)
        {
            yedNodeToEnable.Active = true;
            yedNodeToEnable.DetermineState();
            dungeonOneNode.ActivateNode();
        }

        //Post cutscene
        /*
         * Interact with Yed node. Yed edges out window and takes ticket. Unlocks train
         * Make node no longer interactable
         */

        else if (!data.DungeonsCompleted[0])
        {

        }

        //Post dungeon 1
        /*
         * Loona makes a snail trail home *map event*
         * Loona house node is enabled *cutscene*
         * Loona grants rock for KC node *map event*
         */

        else if (!data.LoonaTalkedToPostDungeonOne)
        {

        }

        else if (!data.MoonStonePlaced)
        {

        }

        else if (!data.DungeonsCompleted[1])
        {

        }

        //Post dungeon 2
        /*
         * Chlamydia gives the party a bag in dungeon cutscene??
         * Party brings bag to Klepto's house *cutscene*
         * Interactive node where dog coughs out second magical bag where long dog pulls out small dog that stretches across to D3 *map event*
         */

        else if (!data.KleptorTalkedToPostDungeonTwo)
        {

        }

        else if (!data.SmallDogStretched)
        {

        }

        else if (!data.DungeonsCompleted[2])
        {

        }

        //Post dungeon 3
        /*
         * Small dog is now a skeleton because he died
         * Orbnus explodes => schnoze moves into place making d4 accessable *map event*
         * Loon-dog :penguin-dance: south of ldg node SW of D3. Unlocks D4 *cutscene*
         */

        else if (!data.LoonaTalkedToPostDungeonThree)
        {

        }

        else if (!data.DungeonsCompleted[3])
        {

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

        }

        else
        {

        }
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

    public void SaveOlarisPrelude()
    {
        olarisHomeNode.DeactiveNode();
        data.PreludeOlarisTalkedTo = true;
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
        CurrentNode.Interactive = false;
        CurrentNode.DetermineState();
        klipsolRockReceiver.sprite = klipsolRockInserted;

        OverworldCharacter character = FindObjectOfType<OverworldCharacter>();
        character.enabled = false;

        while(klipsolRock.position != klipsolRockDesination.position)
        {
            klipsolRock.position = Vector3.MoveTowards(klipsolRock.position, klipsolRockDesination.position, klipsolRockMoveSpeed * Time.deltaTime);
            yield return null;
        }

        d3Node.Active = true;
        d3Node.DetermineState();
        CurrentNode.SetupNode();

        character.enabled = true;

        //save data
        data.MoonStonePlaced = true;
        SaveManager.SaveDimensionOne(data);
    }
}
