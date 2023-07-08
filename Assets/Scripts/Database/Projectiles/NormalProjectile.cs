using System.Collections;
using System.Collections.Generic;
using TowerDefense.Pooling;
using TowerDefense.Projectiles;
using UnityEngine;

namespace TowerDefense.Database
{
    [CreateAssetMenu(fileName = "Normal Projectile", menuName = "Tower-Defense/Projectile/Normal", order = 0)]
    public class NormalProjectile : Projectile
    {
        public override void Shoot<T>(T source)
        {
            var enemy = source.GetEnemyTarget();

            var _sourcePos = source.ProjectileSpot.position;
            var _sourceRot = Quaternion.LookRotation(enemy.transform.position - _sourcePos);

            if (muzzlePrefab)
                ObjectPool.GetPool(muzzlePrefab, _sourcePos, _sourceRot);

            if (projectilePrefab)
            {
                var _projectile = ObjectPool.GetPool(projectilePrefab, _sourcePos, _sourceRot);

                _projectile.GetComponent<ProjectileMover>().Initialize(source.AttackDamage * multipleDamage, projectileSpeed);
                _projectile.GetComponent<ProjectileMover>().Shoot(enemy);
            }
        }

        public override void Dispose()
        {

        }
    }
}
