using System.Collections.Generic;
using UnityEngine;
using System.Collections;

namespace TowerDefense.Projectiles
{
    using Pooling;
    using Database;

    public class ProjectileMover : PoolRelease
    {
        [SerializeField]
        private GameObject impactPrefab;

        private Effect effect;

        private float speed = 0;

        private float baseDamage = 0;
        private float finalDamage = 0;

        private float effectRange = 0;

        private ParticleSystem particle;

        private IEnumerator projectileMover;

        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
        }

        public void Initialize(Effect effect, float effectRange, float baseDamage, float finalDamage, float speed)
        {
            this.effect = effect;
            this.baseDamage = baseDamage;
            this.effectRange = effectRange;

            this.speed = speed;
            this.finalDamage = finalDamage;
        }

        public void Shoot(Enemy target)
        {
            if (projectileMover != null)
                StopCoroutine(projectileMover);
            StartCoroutine(projectileMover = Mover(target));
        }

        private IEnumerator Mover(Enemy target)
        {
            while (Vector3.Distance(target.transform.position, transform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);

                var _rotate = target.transform.position - transform.position;
                if (_rotate != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(_rotate);

                yield return null;
            }

            var impactAOE = ObjectPool.GetPool(impactPrefab, target.transform.position, Quaternion.identity).GetComponent<AreaOfEffect>();

            if (!impactAOE)
            {
                ((IDamageable)target).Damage(finalDamage);
                effect?.ApplyEffect(baseDamage, new Enemy[] { target });
            }
            else
            {
                impactAOE.transform.localScale = Vector3.one;
                impactAOE.Initialize(effect, baseDamage, finalDamage, effectRange);
                impactAOE.ApplyEffect();
            }


            Release();
        }
    }
}