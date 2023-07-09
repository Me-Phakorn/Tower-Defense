using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

namespace TowerDefense.Projectiles
{
    using Pooling;
    using Database;

    public class AreaOfEffect : PoolRelease
    {
        private Effect effect;

        private float baseDamage = 0;
        private float finalDamage = 0;
        private float effectRange = 0;

        private ParticleSystem particle;

        private IEnumerator delayRelease;

        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
        }

        public void Initialize(Effect effect, float baseDamage, float finalDamage, float effectRange)
        {
            this.effect = effect;

            Initialize(baseDamage, finalDamage, effectRange);
        }

        public void Initialize(float baseDamage, float finalDamage, float effectRange)
        {
            this.baseDamage = baseDamage;
            this.finalDamage = finalDamage;
            this.effectRange = effectRange;
        }

        public void ApplyEffect()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, effectRange, transform.up);
            Enemy[] enemies = hits.Where(h => h.collider.CompareTag("Enemy")).Select(h => h.collider.GetComponent<Enemy>()).ToArray();

            if (enemies.Length == 0)
                return;

            foreach (var enemy in enemies)
                enemy.Damage(finalDamage);

            effect?.ApplyEffect(baseDamage, enemies);

            if (delayRelease != null)
                StopCoroutine(delayRelease);
            StartCoroutine(delayRelease = DelayRelease());
        }

        private IEnumerator DelayRelease()
        {
            yield return new WaitForSeconds(particle.main.duration + 0.5f);

            Release();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, effectRange);
        }
    }
}