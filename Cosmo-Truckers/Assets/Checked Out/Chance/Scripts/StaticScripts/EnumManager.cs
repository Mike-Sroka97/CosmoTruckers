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
        All_Target
    }

    public enum NodeType
    {
        CombatNode, //Easy just combat
        NCNode_SingleRandomPlayerAug, //Adds aug to a single random player
        NCNode_PlayerOrderChoiceAug, //Each player choices and AUG based on turn order
        RestNode, //Reset players HP
        BossNode, //Same as combat node

        //TODO
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
}
