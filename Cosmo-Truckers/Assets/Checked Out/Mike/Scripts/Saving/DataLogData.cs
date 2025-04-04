using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataLogData
{
    //player data files
    public Dictionary<string, bool> DataFiles = new Dictionary<string, bool>();
    public Dictionary<string, bool> StaffSummonsDataFiles = new Dictionary<string, bool>();

    //dimension data files
    //tutorial (always unlocked)
    public Dictionary<string, bool> TutorialDataFiles = new Dictionary<string, bool>();

    //dimension 1
    public Dictionary<string, bool> DimensionOneDataFiles = new Dictionary<string, bool>();

    //dimension 2
    //dimension 3
    //dimension 4
    //dimension 5
    //dimension 6
    //dimension 7
    //dimension 8
    //dimension Final

    /// <summary>
    /// Set defaults if they haven't been before
    /// </summary>
    public void InitialSetup()
    {
        if (DataFiles.Count == 0)
            SetupDataFiles();
    }

    /// <summary>
    /// Resets all data
    /// </summary>
    public void ResetData()
    {
        DataFiles.Clear();
        InitialSetup();
    }

    /// <summary>
    /// Default Cowork Data Files
    /// </summary>
    private void SetupDataFiles()
    {
        //Coworkers
        DataFiles.Add("INA", true);
        DataFiles.Add("Toblerone", true);
        DataFiles.Add("Aeglar", true);
        DataFiles.Add("Proto", true);
        DataFiles.Add("Safe-T", true);
        DataFiles.Add("Six-Face", true);
        DataFiles.Add("Long Dog", false);
        DataFiles.Add("TBD1", false);
        DataFiles.Add("TBD2", false);
        DataFiles.Add("TBD3", false);

        //Friendly Summons
        DataFiles.Add("Foodies", true);

        //Tutorial
        DataFiles.Add("Malite", true);
        DataFiles.Add("Malice", true);
        DataFiles.Add("Gucking Company", true);

        //NPCs
        DataFiles.Add("Olaris", false);
        DataFiles.Add("Yed", false);
        DataFiles.Add("Luna", false);
        DataFiles.Add("Kleptor", false);
        DataFiles.Add("Klipsol", false);
        DataFiles.Add("Dead Dog", false);

        //Enemies
        DataFiles.Add("Amber Star", false);
        DataFiles.Add("Nebular Cloud", false);
        DataFiles.Add("Walking Noise", false);
        DataFiles.Add("Dutiful Dross", false);
        DataFiles.Add("Gloom Guy", false);
        DataFiles.Add("Cosmic Crust", false);
        DataFiles.Add("Wying Dreem", false);
        DataFiles.Add("Meat Tonguer", false);
        DataFiles.Add("Tralaxy", false);
        DataFiles.Add("Demofongo", false);
        DataFiles.Add("Orbnus", false);
        DataFiles.Add("Qmuav", false);

        //Enemy Summons
        DataFiles.Add("Astron", false);
        DataFiles.Add("Demopaw", false);
        DataFiles.Add("Atlas Stone", false);
        DataFiles.Add("Slorbnus", false);
        DataFiles.Add("Galaster", false);
        DataFiles.Add("Ilk of the Singularity", false);
        DataFiles.Add("Timothy", false);

        //Dungeon Friendly Summons
        DataFiles.Add("Empty Nova", false);
        DataFiles.Add("Dulaxy", false);
    }
}
