using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class COKO_PieSegment : MonoBehaviour
{
    private void Start()
    {
        Handles.DrawSolidArc(transform.position, Vector3.zero, Vector3.right, 45f, transform.localScale.x);
    }
}
