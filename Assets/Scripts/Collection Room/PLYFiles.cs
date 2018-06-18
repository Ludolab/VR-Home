using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PLYFiles
{

	public static void WritePLY(string filePath, ParticleSystem.Particle[] particles)
    {
        using (StreamWriter file = new StreamWriter(filePath))
        {
            int numParticles = particles.Length;

            string header1 = @"ply
format ascii 1.0
element vertex ";
            string header2 = @"property float32 x
property float32 y
property float32 z
property uchar red
property uchar green
property uchar blue
end_header";
            file.WriteLine(header1 + numParticles);
            file.WriteLine(header2);

            foreach (ParticleSystem.Particle particle in particles)
            {
                Vector3 pPos = particle.position;
                string particlePosInfo = pPos.x + " " + pPos.y + " " + pPos.z + " ";
                Color32 pCol = particle.startColor;
                string particleColInfo = pCol.r + " " + pCol.g + " " + pCol.b + " ";
                file.WriteLine(particlePosInfo + particleColInfo);
            }
        }
    }

    public static ParticleSystem.Particle[] ReadPLY(string filePath, float pointsSize)
    {
        ParticleSystem.Particle[] particles;

        using (StreamReader file = new StreamReader(filePath))
        {
            file.ReadLine();
            file.ReadLine();
            string vertexInfo = file.ReadLine();
            int numParticles = int.Parse(vertexInfo.Split(' ')[2]);
            particles = new ParticleSystem.Particle[numParticles];

            string line = file.ReadLine();
            while (line != "end_header")
            {
                line = file.ReadLine();
            }
            for (int i = 0; i < numParticles; i++)
            {
                ParticleSystem.Particle particle = new ParticleSystem.Particle();
                string[] particleInfo = file.ReadLine().Split(' ');

                float pX = float.Parse(particleInfo[0]);
                float pY = float.Parse(particleInfo[1]);
                float pZ = float.Parse(particleInfo[2]);
                particle.position = new Vector3(pX, pY, pZ);
                particle.startSize = pZ * pointsSize * 0.02f;

                if (particleInfo.Length >= 6)
                {
                    byte pcR = byte.Parse(particleInfo[3]);
                    byte pcG = byte.Parse(particleInfo[4]);
                    byte pcB = byte.Parse(particleInfo[5]);
                    particle.startColor = new Color32(pcR, pcG, pcB, 255);
                }
                else
                {
                    particle.startColor = Color.white;
                }

                particles[i] = particle;
            }
        }
        return particles;
    }
}
