using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSpawnParticle : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        SpawnManager.Instance.DeSpawnParticle(GetComponent<ParticleSystem>());
    }
}
