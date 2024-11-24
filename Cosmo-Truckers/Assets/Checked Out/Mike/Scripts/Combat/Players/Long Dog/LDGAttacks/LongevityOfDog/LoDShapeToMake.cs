using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoDShapeToMake : MonoBehaviour
{
    public void UpdateShapeColors(Color colorToUpdate)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = colorToUpdate;
        }
    }
}
