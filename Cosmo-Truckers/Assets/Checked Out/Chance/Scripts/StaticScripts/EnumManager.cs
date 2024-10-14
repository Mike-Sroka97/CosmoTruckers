using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager
{
    public enum TargetingType
    {
        No_Target,
        Self_Target,
        Single_Target,
        Multi_Target_Cone,
        Multi_Target_Choice,
        AOE,
        All_Target,
        Enemy_Intentions
    }

    public enum NodeType
    {
        CombatNode, //Easy just combat
        NCNode_SingleRandomPlayerAug, //Adds aug to a single random player
        NCNode_PlayerOrderChoiceAug, //Each player choices and AUG based on turn order
        RestNode, //Reset players HP
        BossNode, //Same as combat node

        NCNode_PlayerDependent,
        NCNode_PartyVoting,
        NCNode_PartyDistribution,
        NCNode_Auto
    }

    public enum NCNodeValue
    {
        NA,
        Positive,
        Negative,
        Neutral,
    }

    public enum CardinalDirections
    {
        Up,
        Left,
        Down,
        Right
    }

    public enum CharacterID
    {
        Aeglar,
        Proto,
        SafeT,
        SixFace,
        LongDog,
    }

    public enum CombatOutcome
    {
        Damage, 
        Healing, 
        MultiDamage, 
        MultiHealing,
        Shield, 
        Resurrect,
    }

    public enum ColorPalette
    {
        White,
        Gray,
        DarkGray,
        Black,
        Yellow,
        Lime,
        Green,
        DarkGreen,
        Orange,
        DarkOrange,
        DarkBlue,
        DarkPurple,
        Magenta,
        Pink,
        Blue,
        Purple,
        Orchid,
        Beige,
        LightBlue,
        LightPurple,
        NeonRed,
        NeonGreen,
        NeonOrange,
        NeonYellow,
        NeonWhite,
    }

    public static readonly Dictionary<ColorPalette, Color32> Colors = new Dictionary<ColorPalette, Color32>
    {
        //Color palette colors
        {ColorPalette.White, new Color32(237,232,197,255)},
        {ColorPalette.Gray, new Color32(189,157,157,255)},
        {ColorPalette.DarkGray, new Color32(130,91,112,255)},
        {ColorPalette.Black, new Color32(48,17,28,255)},
        {ColorPalette.Lime, new Color32(157,189,92,255)},
        {ColorPalette.Green, new Color32(93,153,92,255)},
        {ColorPalette.DarkGreen, new Color32(77,105,99,255)},
        {ColorPalette.Yellow, new Color32(240,211,117,255)},
        {ColorPalette.Orange, new Color32(235,148,101,255)},
        {ColorPalette.DarkOrange, new Color32(189,104,91,255)},
        {ColorPalette.DarkBlue, new Color32(120,182,204,255)},
        {ColorPalette.DarkPurple, new Color32(87,51,89,255)},
        {ColorPalette.Magenta, new Color32(212,76,121,255)},
        {ColorPalette.Pink, new Color32(250,150,170,255)},
        {ColorPalette.Blue, new Color32(114,127,181,255)},
        {ColorPalette.Purple, new Color32(145,83,163,255)},
        {ColorPalette.Orchid, new Color32(138,59,102,255)},
        {ColorPalette.Beige, new Color32(240,179,149,255)},
        {ColorPalette.LightBlue, new Color32(100,80,148,255)},
        {ColorPalette.LightPurple, new Color32(182,137,204,255)},

        //Combat colors
        {ColorPalette.NeonRed, new Color32(191,0,0,255)},
        {ColorPalette.NeonGreen, new Color32(57,255,20,255)},
        {ColorPalette.NeonOrange, new Color32(255,95,31,255)},
        {ColorPalette.NeonYellow, new Color32(255,234,0,255)},
        {ColorPalette.NeonWhite, new Color32(191,191,191,255)},
    };
}
