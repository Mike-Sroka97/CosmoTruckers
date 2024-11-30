using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererCollider : MonoBehaviour
{
    TrailRenderer myTrail;
    [SerializeField] EdgeCollider2D myCollider;
    [SerializeField] int colliderBuffer;
    [SerializeField] Vector2 inaPosition = new Vector2(100, 100); 

    private void Awake()
    {
        myTrail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        SetColliderPointsFromTrail(myTrail, myCollider);
    }

    private void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();
        
        for(int i = 0; i < trail.positionCount; i++)
        {
            points.Add(trail.GetPosition(i) - (Vector3)inaPosition);
        }

        for (int i = 0; i < colliderBuffer; i++)
        {
            if (points.Count <= 0)
                break;
            points.RemoveAt(0);
        }

        collider.SetPoints(points);
    }
}
