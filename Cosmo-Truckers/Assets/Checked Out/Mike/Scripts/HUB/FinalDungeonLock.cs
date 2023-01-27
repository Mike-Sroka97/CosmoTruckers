using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDungeonLock : MonoBehaviour
{
    private void Start()
    {
        //If the host does not have the requirements for the final dungeon to be unlocked, lock this tile in the hub
        gameObject.tag = "Untagged";
    }
}
