using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticles : MonoBehaviour
{
    public ParticleSystem[] parentParticles;

    List <ParticleSystem> allParticles = new List <ParticleSystem>();


    // Start is called before the first frame update
    void Start()
    {
        GetAllParticles();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (var particle in allParticles)
            {
                var emission = particle.emission;

                emission.enabled = !emission.enabled; 

                if (!emission.enabled)
                {
                    particle.Play();
                }
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
