using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class DimensionData
{
    public bool[] DungeonsCompleted = new bool[4]; //four dungeons
    public string PreviousNode = "Node Spawn"; // name of last node that the player was on

    public abstract void SaveLevelData();
    public abstract void LoadLevelData();

    public abstract void DeleteLevelData();
}
