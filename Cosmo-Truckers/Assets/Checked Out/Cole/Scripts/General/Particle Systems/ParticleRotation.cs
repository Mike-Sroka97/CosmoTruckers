using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

//This script goes on a particle system that needs its rotation updated manually
//It works with layered particles on one system
//EX: SFT Gonzor Ring. It has no velocity but needs to be rotated 
public class ParticleRotation : MonoBehaviour
{
    List<ParticleSystem> allParticles = new List<ParticleSystem>();

    public void SetParticleRotations(float startRotation, bool playAfter)
    {
        //Unity inspector forumula: val * 180 / pi
        //I want my value. Reverse this
        float newRotation = startRotation / 180f * Mathf.PI; 

        //If the parent is a particleSystem, which it should be
        if (gameObject.GetComponent<ParticleSystem>() != null)
        {
            allParticles.Add(gameObject.GetComponent<ParticleSystem>());
            var particleMain = gameObject.GetComponent<ParticleSystem>().main;
            particleMain.startRotationZ = newRotation;
        }

        //Get count of all of its children
        int children = transform.childCount;

        //Loop through its children, add them to list
        for (int i = 0; i < children; i++)
        {
            List<ParticleSystem> allParticles = new List<ParticleSystem>();

            Transform _child = transform.GetChild(i);

            if (_child.GetComponent<ParticleSystem>() != null)
            {
                allParticles.Add(_child.GetComponent<ParticleSystem>());
                var particleMain = _child.GetComponent<ParticleSystem>().main;
                particleMain.startRotationZ = newRotation;
            }
        }

        if (playAfter)
        {
            foreach (ParticleSystem particle in allParticles)
            {
                particle.Play();
            }
        }
    }

}
