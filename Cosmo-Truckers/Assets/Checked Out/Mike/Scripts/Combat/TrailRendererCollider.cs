using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererCollider : MonoBehaviour
{
    TrailRenderer myTrail;
    [SerializeField] EdgeCollider2D myCollider;
    [SerializeField] int colliderBuffer; 

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
            points.Add(trail.GetPosition(i));
        }

        for (int i = 0; i < colliderBuffer; i++)
            points.RemoveAt(0); 

        collider.SetPoints(points);
    }
}
