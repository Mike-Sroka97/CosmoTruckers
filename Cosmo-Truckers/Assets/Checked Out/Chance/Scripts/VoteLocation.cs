using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteLocation : MonoBehaviour
{
    [SerializeField] int Dimension;
    [SerializeField] string DimensionScene;

    public int GetDimension { get => Dimension; }
    public string GetDimensionName { get => DimensionScene; }
}
