using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWaveDamaging : MonoBehaviour
{
    [SerializeField] int maxPoints;
    [SerializeField] float amplitude;
    [SerializeField] float frequency;
    [SerializeField] float moveSpeed;
    [SerializeField] bool startAtTau = false;

    LineRenderer myRenderer;
    EdgeCollider2D myCollider;
    const float tau = 2 * Mathf.PI;
    float currentTime = 0;

    private void Start()
    {
        myRenderer = GetComponent<LineRenderer>();
        myCollider = GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        DrawLine();
    }

    private void DrawLine()
    {
        currentTime += Time.deltaTime;

        float xStart;
        float xFinish;

        xStart = 0;
        xFinish = tau;

        List<Vector2> edges = new List<Vector2>();

        myRenderer.positionCount = maxPoints;
        for(int i = 0; i < maxPoints; i++)
        {
            float progress = (float)i / (maxPoints - 1);
            float x;

            x = Mathf.Lerp(xStart, xFinish, progress);


            float y;
            if(!startAtTau)
            {
                y = amplitude * Mathf.Sin((tau * frequency * x) + currentTime * moveSpeed);
            }
            else
            {
                y = amplitude * Mathf.Sin((-tau * frequency * x) + currentTime * moveSpeed);
            }
            myRenderer.SetPosition(i, new Vector3(x, y, 0));
            edges.Add(new Vector2(x, y));
        }

        myCollider.SetPoints(edges);
    }
}
