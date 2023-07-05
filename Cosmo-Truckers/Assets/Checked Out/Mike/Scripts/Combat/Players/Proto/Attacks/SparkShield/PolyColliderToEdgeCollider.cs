using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyColliderToEdgeCollider : MonoBehaviour
{
    private void Awake()
    {
        PolygonCollider2D myCollider = GetComponent<PolygonCollider2D>();
        List<Vector2> points = new List<Vector2>();
        foreach (Vector2 p in myCollider.points)
            points.Add(p);

        points.Add(new Vector2(points[0].x, points[0].y));
        EdgeCollider2D myNewCollider = gameObject.AddComponent<EdgeCollider2D>();
        myNewCollider.points = points.ToArray();
        Destroy(myCollider);
    }
}
