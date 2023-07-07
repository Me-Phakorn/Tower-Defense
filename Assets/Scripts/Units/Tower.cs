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
        [SerializeField, Range(0.1f, 5f)]
        private float attackRange = 1;

        [Header("Projectile Setting")]
        [SerializeField]
        private Transform projectileSpot;
        [SerializeField]
        private Projectile projectile;

        public bool IsPause { get; set; } = false;

        public bool IsEnemies { get => enemies != null && enemies.Count > 0; }

        public int AttackDamage => attackDamage;

        public float FireRate => fireRate;
        public float AttackRange => attackRange;

        public Transform ProjectileSpot => projectileSpot;

        private List<Enemy> enemies;

        private float shootTimer = 0;

        public void Construction(ITowerSetting setting)
        {
            enemies = new List<Enemy>();

            attackDamage = setting.AttackDamage;
            attackRange = setting.AttackRange;
            fireRate = setting.FireRate;

            var _Range = GetComponent<SphereCollider>();
            _Range.radius = attackRange * GameSetting.MULTIPLE_RANGE;

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
            if (shootTimer >= FireRate)
                ExecuteProjectile();
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
