using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCraftingCharacterData : MonoBehaviour
{
    [Serializable]
    public class CharacterSpellData
    {
        public string[] specs;
        public string[] spells;
    }
}
