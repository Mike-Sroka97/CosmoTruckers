using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DimensionData
{
    public bool FirstTimeVisit;
    public bool[] DungeonsCompleted;

    public abstract void SaveLevelData();
    public abstract void LoadLevelData();
}
