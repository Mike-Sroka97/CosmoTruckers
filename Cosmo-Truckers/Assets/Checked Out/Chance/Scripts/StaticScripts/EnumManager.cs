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
        CombatNode,
        NCNode_SingleRandomPlayerAug,
        NCNode_PlayerOrderChoiceAug,
        RestNode,
        BossNode,
    }
    public enum NCNodeValue
    {
        NA,
        Positive,
        Negative,
        Neutral,
    }
}
