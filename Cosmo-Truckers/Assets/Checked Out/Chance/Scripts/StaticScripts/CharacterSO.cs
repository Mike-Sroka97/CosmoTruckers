using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character")]
public class CharacterSO : ScriptableObject
{
    public int PlayerID;
    public string CharacterName;
    public Sprite CharacterImage;
}
