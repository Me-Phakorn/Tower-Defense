using System.Collections;
using UnityEngine;

namespace TowerDefense.Pooling
{
    public class ParticleAutoRelease : PoolRelease
    {
        private ParticleSystem particle;

        private IEnumerator delayRelease;

        private void OnEnable()
        {
            if (!particle)
                particle = GetComponent<ParticleSystem>();

            if (delayRelease != null)
                StopCoroutine(delayRelease);
            StartCoroutine(delayRelease = DelayRelease());
        }

        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
        }

        private IEnumerator DelayRelease()
        {
            yield return new WaitUntil(() => !particle.isPlaying);
            
            Release();
        }
    }
}