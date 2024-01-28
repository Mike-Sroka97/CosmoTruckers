using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ORBTitanCrusher : Crusher
{
    [SerializeField] ParticleSystem speedLines;
    
    ObjectShaker armShaker;
    Vector3 armStartingPosition;
    float correctionSpeed = 1f; 

    private void Awake()
    {
        armShaker = GetComponentInChildren<ObjectShaker>();
        armStartingPosition = armShaker.transform.localPosition; 
    }

    private void Update()
    {
        SetParticleState(); 
        SetShakeState();
    }

    void SetParticleState()
    {
        if (retracting)
        {
            speedLines.gameObject.SetActive(false);
        }
        else
        {
            speedLines.gameObject.SetActive(true);
        }
    }

    void SetShakeState()
    {
        if (engaging)
        {
            armShaker.SetShakeState(true); 
        }
        else
        {
            armShaker.SetShakeState(false);
            armShaker.transform.localPosition = armStartingPosition;
        }
    }
}
