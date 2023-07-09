using System.Collections;
using System.Collections.Generic;
using TowerDefense.Pooling;
using TowerDefense.Projectiles;
using UnityEngine;

namespace TowerDefense.Database
{
    [CreateAssetMenu(fileName = "Bomb Projectile", menuName = "Tower-Defense/Projectile/Bomb", order = 0)]
    public class BombProjectile : Projectile
    {
        [Header("Bomb Setting")]
        [SerializeField, Range(1, 5)]
        private float bombRange;
        [SerializeField, Range(1, 300)]
        private float damagePercent;

        public override void Shoot<T>(T source)
        {
            var enemy = source.GetEnemyTarget();

            if (!enemy)
                return;

            var _sourcePos = source.ProjectileSpot.position;
            var _sourceRot = Quaternion.LookRotation(enemy.transform.position - _sourcePos);

            if (muzzlePrefab)
                ObjectPool.GetPool(muzzlePrefab, _sourcePos, _sourceRot);

            if (projectilePrefab)
            {
                var _projectile = ObjectPool.GetPool(projectilePrefab, _sourcePos, _sourceRot);

                _projectile.GetComponent<ProjectileMover>().Initialize(effect, bombRange,
                    source.AttackDamage, (source.AttackDamage * multipleDamage) * (damagePercent / 100), projectileSpeed);
                _projectile.GetComponent<ProjectileMover>().Shoot(enemy);
            }
        }

        public override void Dispose()
        {

        }
    }
}
