using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dungeon", menuName = "ScriptableObjects/Dungeon")]
public class DungeonSO : ScriptableObject
{
    public Sprite DungonSprite;
    [Tooltip("Same name as the scene for this dungeon")]public string DungeonName;
    [TextArea(5,10)]public string DungeonDescription;
}
