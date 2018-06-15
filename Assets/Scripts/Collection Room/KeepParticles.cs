using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepParticles : MonoBehaviour
{

    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[0];

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void SetParticles(ParticleSystem.Particle[] particles)
    {
        this.particles = particles;
    }

    private void Update()
    {
        particleSystem.SetParticles(particles, particles.Length);
    }
}
