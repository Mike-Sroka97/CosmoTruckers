using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    [SerializeField] float g = 1f;
    static float G;
    public static List<Rigidbody2D> attractors = new List<Rigidbody2D>();
    public static List<Rigidbody2D> attractees = new List<Rigidbody2D>();
    public static bool isSimulatingLive = true;

    public void Initialize()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach(Player player in players)
        {
            Rigidbody2D[] playerBodies = player.GetComponentsInChildren<Rigidbody2D>();
            foreach(Rigidbody2D playerBody in playerBodies)
            {
                Graviton playerGraviton = playerBody.gameObject.AddComponent<Graviton>();
                playerGraviton.IsAttractee = true;
                playerGraviton.IsAttractor = false;
            }
        }
    }
    void FixedUpdate()
    {
        G = g;
        if (isSimulatingLive)
            SimulateGravities();
    }
    public static void SimulateGravities()
    {
        foreach (Rigidbody2D attractor in attractors)
        {
            foreach (Rigidbody2D attractee in attractees)
            {
                if (attractor != attractee)
                    AddGravityForce(attractor, attractee);
            }
        }
    }

    public static void AddGravityForce(Rigidbody2D attractor, Rigidbody2D target)
    {
        float massProduct = attractor.mass * target.mass * G;

        Vector3 difference = attractor.position - target.position;
        float distance = difference.magnitude;

        float unScaledforceMagnitude = massProduct / Mathf.Pow(distance, 2);
        float forceMagnitude = G * unScaledforceMagnitude;

        Vector3 forceDirection = difference.normalized;

        Vector3 forceVector = forceDirection * forceMagnitude;

        target.AddForce(forceVector);
    }
}
