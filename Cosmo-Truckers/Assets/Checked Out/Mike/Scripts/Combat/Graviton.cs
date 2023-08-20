using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graviton : MonoBehaviour
{
    Rigidbody2D rigidBody;
    public bool IsAttractee//property 
    {
        get
        {
            return isAttractee;
        }
        set
        {
            if (value == true)
            {
                if (!GravityManager.attractees.Contains(this.GetComponent<Rigidbody2D>()))
                {
                    GravityManager.attractees.Add(rigidBody);
                }
            }
            else if (value == false)
            {
                GravityManager.attractees.Remove(rigidBody);
            }
            isAttractee = value;
        }
    }
    public bool IsAttractor//property
    {
        get
        {
            return isAttractor;
        }
        set
        {
            if (value == true)
            {
                if (!GravityManager.attractors.Contains(this.GetComponent<Rigidbody2D>()))
                    GravityManager.attractors.Add(rigidBody);
            }
            else if (value == false)
            {
                GravityManager.attractors.Remove(rigidBody);
            }
            isAttractor = value;
        }
    }
    [SerializeField] bool isAttractor;//field
    [SerializeField] bool isAttractee;//field

    [SerializeField] Vector3 initialVelocity;
    [SerializeField] bool applyInitialVelocityOnStart;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        IsAttractor = isAttractor;
        IsAttractee = isAttractee;
    }
    void Start()
    {
        if (applyInitialVelocityOnStart)
        {
            ApplyVelocity(initialVelocity);
        }

    }
    void OnDisable()
    {
        GravityManager.attractors.Remove(rigidBody);
        GravityManager.attractees.Remove(rigidBody);
    }
    void ApplyVelocity(Vector3 velocity)
    {
        rigidBody.AddForce(initialVelocity, ForceMode2D.Impulse);
    }
}
