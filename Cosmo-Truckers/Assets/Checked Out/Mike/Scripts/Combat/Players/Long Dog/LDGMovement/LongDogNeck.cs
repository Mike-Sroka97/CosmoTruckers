using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogNeck : MonoBehaviour
{
    
    [SerializeField] LineRenderer myLineRenderer;
    [SerializeField] EdgeCollider2D myCollider;
    [SerializeField] float myEdgeRadius;
    [SerializeField] Texture rightFacingNeck;
    [SerializeField] Texture leftFacingNeck;

    List<Vector2> linePoints = new List<Vector2>();
    int pointCount = 0;
    float minDistanceBetweenPoints = .2f;

    LongDogINA dog;
    PlayerBody myBody;

    private void Start()
    {
        dog = transform.parent.GetComponent<LongDogINA>();
        myLineRenderer.material = dog.MyRenderers[0].material;
        myBody = GetComponent<PlayerBody>();

        if (dog.GetHead().transform.eulerAngles.y == 0)
            myLineRenderer.material.mainTexture = leftFacingNeck;
        else
            myLineRenderer.material.mainTexture = rightFacingNeck;

        myBody.Body = dog;
    }

    public void AddPoint(Vector2 point)
    {
        if(pointCount >= 1 && Vector2.Distance(point, GetLastPoint()) < minDistanceBetweenPoints)
        {
            return;
        }

        if(linePoints.Count == 0)
            dog.SetupLineRenderer(this);

        linePoints.Add(point);
        pointCount++;

        myLineRenderer.positionCount = pointCount;
        myLineRenderer.SetPosition(pointCount - 1, point);

        if(pointCount > 1)
        {
            myCollider.points = linePoints.ToArray();
        }
    }

    public void RemovePoint()
    {
        linePoints.RemoveAt(0);
        pointCount--;
        myLineRenderer.positionCount = pointCount;
        List<Vector3> newVertices = new List<Vector3>();
        foreach (Vector2 p in linePoints)
        {
            newVertices.Add(p);
        }
        myLineRenderer.SetPositions(newVertices.ToArray());
        myCollider.points = linePoints.ToArray();
    }

    public Vector2 GetLastPoint()
    {
        return myLineRenderer.GetPosition(pointCount - 1);
    }

    public void SetLineColor(Gradient color)
    {
        myLineRenderer.colorGradient = color;
    }

    public void SetPointsMinDistance(float distance)
    {
        minDistanceBetweenPoints = distance;
    }

    public void SetLineWidth(float width)
    {
        myLineRenderer.startWidth = width;
        myLineRenderer.endWidth = width;

        myCollider.edgeRadius = myEdgeRadius;  
    }

    public Gradient GetLineColor() { return myLineRenderer.colorGradient; }
    public int GetPointCount() { return pointCount; }
    public List<Vector2> GetPointList() { return linePoints;}
}
