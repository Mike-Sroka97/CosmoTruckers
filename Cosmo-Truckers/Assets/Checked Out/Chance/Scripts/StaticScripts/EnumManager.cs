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
    }
}
