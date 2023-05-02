using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDogNeck : MonoBehaviour
{
    
    [SerializeField] LineRenderer myLineRenderer;
    [SerializeField] EdgeCollider2D myCollider;

    List<Vector2> linePoints = new List<Vector2>();
    int pointCount = 0;
    //float circleColliderRadius;
    float minDistanceBetweenPoints = .2f;

    public void AddPoint(Vector2 point)
    {
        if(pointCount >= 1 && Vector2.Distance(point, GetLastPoint()) < minDistanceBetweenPoints)
        {
            return;
        }

        linePoints.Add(point);
        pointCount++;

        //CircleCollider2D circleCollider = this.gameObject.AddComponent<CircleCollider2D>();
        //circleCollider.offset = point;
        //circleCollider.radius = circleColliderRadius;


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
        foreach(Vector2 p in linePoints)
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

        //circleColliderRadius = width / 2f;
        myCollider.edgeRadius = width / 2f; 
    }

    public int GetPointCount() { return pointCount; }
    public List<Vector2> GetPointList() { return linePoints;}
}
