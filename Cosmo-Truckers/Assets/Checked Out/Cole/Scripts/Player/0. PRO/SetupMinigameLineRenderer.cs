using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupMinigameLineRenderer : MonoBehaviour
{
    public void SetLineLocalPositions(CombatMove minigame)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 indexPosition = lineRenderer.GetPosition(i);
            indexPosition += minigame.transform.position; 
            lineRenderer.SetPosition(i, indexPosition);
        }

        lineRenderer.enabled = true; 
    }
}
