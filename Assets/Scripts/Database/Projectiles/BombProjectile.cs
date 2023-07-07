using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Database
{
    [CreateAssetMenu(fileName = "Bomb Projectile", menuName = "Tower-Defense/Projectile/Bomb", order = 0)]
    public class BombProjectile : Projectile
    {
        [Header("Bomb Setting")]
        [SerializeField, Range(2, 5)]
        private float bombRange = 2;
        [SerializeField, Range(1, 300)]
        private float damagePercent = 50;

        public override void Shoot<T>(T source)
        {
            
        }

        public override void Dispose()
        {

        }
    }
}
