using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TowerDefense.Projectiles
{
    using Pooling;
    public class ProjectileMover : PoolRelease
    {
        [SerializeField]
        private GameObject impactPrefab;
        
        [SerializeField]
        private Ease projectileEase;

        private ParticleSystem particle;

        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
        }

        void Shoot(IDamageable target)
        {
            // transform.DOMove(_destination, setting.ProjectileDuration).SetEase().OnComplete(() =>
            // {
            //     if (impactPrefab)
            //         AbilitiesPool.GetPool(impactPrefab, _destination, Quaternion.identity);

            //     ((IDamageable)target).Damage(source.Attack);

            //     Release();

            //     source.isAbilityPerform = false;
            // });
        }
    }
}