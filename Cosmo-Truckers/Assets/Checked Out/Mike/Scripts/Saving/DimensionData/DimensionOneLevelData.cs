using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionOneLevelData : DimensionData
{
    public bool PreludeOlarisTalkedTo;
    public bool PreludeYedTalkedTo;

    public override void SaveLevelData()
    {
        SaveManager.SaveDimensionOne(this);
    }

    public override void LoadLevelData()
    {
        DimensionOneLevelData loadData = SaveManager.LoadDimensionOne();

        FirstTimeVisit = loadData.FirstTimeVisit;
        DungeonsCompleted = loadData.DungeonsCompleted;
        PreludeOlarisTalkedTo = loadData.PreludeOlarisTalkedTo;
        PreludeYedTalkedTo = loadData.PreludeYedTalkedTo;
    }
}
