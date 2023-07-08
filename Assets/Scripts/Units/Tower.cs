using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    using Database;

    public class Tower : MonoBehaviour, ITowerSetting
    {
        [Header("Tower Setting")]
        [SerializeField, Range(1, 10)]
        private int attackDamage = 1;
        [SerializeField, Range(0.1f, 5f)]
        private float fireRate = 1;
        [SerializeField, Range(3, 7)]
        private int attackRange = 3;

        [Header("Projectile Setting")]
        [SerializeField]
        private Transform projectileSpot;
        [SerializeField]
        private Projectile projectile;

        public bool IsPause { get; set; } = false;

        public bool IsEnemies { get => enemies != null && enemies.Count > 0; }

        public int AttackDamage => attackDamage;
        public int AttackRange => attackRange;

        public float FireRate => fireRate;

        public Transform ProjectileSpot => projectileSpot;

        private List<Enemy> enemies = new List<Enemy>();

        private float shootTimer = 0;

        public void Construction(ITowerSetting setting)
        {
            enemies = new List<Enemy>();

            attackDamage = setting.AttackDamage;
            attackRange = setting.AttackRange;
            fireRate = setting.FireRate;

            var _Range = GetComponent<SphereCollider>();
            _Range.radius = attackRange;

            shootTimer = 0;
        }

        public Enemy GetEnemyTarget()
        {
            return enemies[0];
        }

        private void Update()
        {
            if (IsPause || !IsEnemies)
                return;

            shootTimer += Time.deltaTime;
            if (shootTimer >= fireRate)
            {
                shootTimer = 0;
                ExecuteProjectile();
            }
        }

        private void ExecuteProjectile()
        {
            projectile?.Shoot<Tower>(this);
        }

        private void OnTriggerEnter(Collider enemy)
        {
            if (enemy.CompareTag("Enemy") && enemies != null)
                enemies.Add(enemy.GetComponent<Enemy>());
        }

        private void OnTriggerExit(Collider enemy)
        {
            if (enemy.CompareTag("Enemy") && enemies != null)
                enemies.Remove(enemy.GetComponent<Enemy>());
        }
    }

}
