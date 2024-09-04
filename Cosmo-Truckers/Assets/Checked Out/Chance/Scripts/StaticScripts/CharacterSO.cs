using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character")]
public class CharacterSO : ScriptableObject
{
    public int PlayerID;
    public string CharacterName;
    public Sprite CharacterImage;
    [Header("Combat vars")]
    public GameObject CombatPlayerSpawn;

    [Space(10)]
    public List<Sprite> AltCharacterImages; //Change to Sprites once we have some
    public int SpriteChoice = -1;
}
