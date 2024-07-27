using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DimensionOneLevelData : DimensionData
{
    public bool PreludeOlarisTalkedTo;
    public bool PreludeYedTalkedTo;
    public bool LoonaTalkedToPostDungeonOne;
    public bool MoonStonePlaced;
    public bool KleptorTalkedToPostDungeonTwo;
    public bool SmallDogStretched;
    public bool LoonaTalkedToPostDungeonThree;
    public bool AfterPartyAttended;

    public override void SaveLevelData()
    {
        SaveManager.SaveDimensionOne(this);
    }

    public override void LoadLevelData()
    {
        DimensionOneLevelData loadData = SaveManager.LoadDimensionOne();

        DungeonsCompleted = loadData.DungeonsCompleted;
        PreludeOlarisTalkedTo = loadData.PreludeOlarisTalkedTo;
        PreludeYedTalkedTo = loadData.PreludeYedTalkedTo;
        LoonaTalkedToPostDungeonOne = loadData.LoonaTalkedToPostDungeonOne;
        MoonStonePlaced = loadData.MoonStonePlaced;
        KleptorTalkedToPostDungeonTwo = loadData.KleptorTalkedToPostDungeonTwo;
        SmallDogStretched = loadData.SmallDogStretched;
        LoonaTalkedToPostDungeonThree = loadData.LoonaTalkedToPostDungeonThree;
        AfterPartyAttended = loadData.AfterPartyAttended;
    }
}
