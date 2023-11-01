using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleUpdater : MonoBehaviour
{
    //Only need the parent particle for ones that are nested
    public ParticleSystem[] parentParticles;
    List<ParticleSystem> allParticles = new List<ParticleSystem>();

    void Start()
    {
        GetAllParticles();
    }

    public void SetParticleState(bool enabled)
    {
        foreach (var particle in allParticles)
        {
            var emission = particle.emission;

            if (enabled)
            {
                if (particle.gameObject.activeSelf == false)
                {
                    particle.gameObject.SetActive(true);
                }

                emission.enabled = true;
                particle.Play();
            }
            else
            {
                emission.enabled = false;
            }
        }
    }

    private void GetAllParticles()
    {
        foreach (var particle in parentParticles)
        {
            //Add parent particle
            allParticles.Add(particle);

            //Get count of all of its children
            int children = particle.transform.childCount;

            //Loop through its children, add them to list
            for (int i = 0; i < children; i++)
            {
                Transform _child = particle.transform.GetChild(i);

                if (_child.GetComponent<ParticleSystem>() != null)
                {
                    allParticles.Add(_child.GetComponent<ParticleSystem>());
                }
            }
        }
    }
}
