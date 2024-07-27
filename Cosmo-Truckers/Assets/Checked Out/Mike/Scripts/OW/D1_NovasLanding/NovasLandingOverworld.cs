using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovasLandingOverworld : Overworld
{
    [Header("First time visit stuff")]
    [SerializeField] GameObject olaris;
    [SerializeField] Transform[] olarisPointsToMove;
    [SerializeField] float olarisFadeSpeed;
    [SerializeField] OverworldNode olarisHomeNode;

    [Space(20)]
    [Header("Yed Stuff")]

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
        data = SaveManager.LoadDimensionOne();

        //Pre cutscene
        /*
         * Olaris making back-shots at his daughters front yard. Despondantly returns home *map event*
         * Any node past train is disabled
         * Olaris's home is the only active node in this state 
         */
        if (!data.PreludeOlarisTalkedTo)
        {
            enableLeaderOnFade = false;
        }

        else if (!data.PreludeYedTalkedTo)
        {

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
