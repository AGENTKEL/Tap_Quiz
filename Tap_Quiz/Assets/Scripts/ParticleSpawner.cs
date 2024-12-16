using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private Transform particleParent;
    
    public void SpawnCorrectAnswerParticle(Vector3 spawnPosition)
    {
        if (particlePrefab != null)
        {
            GameObject particle = Instantiate(particlePrefab, spawnPosition, Quaternion.identity, particleParent);
            Destroy(particle, 2f);
        }
    }
}
