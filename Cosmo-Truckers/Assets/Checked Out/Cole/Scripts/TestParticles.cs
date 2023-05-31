using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestParticles : MonoBehaviour
{
    public ParticleSystem[] testParticles; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool particlesEnabled = testParticles[0].emission.enabled;


        if (Input.GetKeyDown(KeyCode.P))
        {
            foreach (var particle in testParticles)
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
}
