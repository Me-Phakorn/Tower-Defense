using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace TowerDefense
{
    using Database;

    public class Tower : MonoBehaviour, ITowerSetting
    {
        [Header("Tower Setting")]
        [SerializeField]
        private EnemyType targetType;
        [SerializeField]
        private TowerAimType aimType;

        [SerializeField, Range(1, 30)]
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

        public EnemyType TargetType => targetType;
        public TowerAimType AimType => aimType;

        private int towerPrice = 0;
        public int TowerPrice => towerPrice;

        public List<Enemy> enemies = new List<Enemy>();

        private float shootTimer = 0;

        public void Construction(ITowerSetting setting)
        {
            enemies = new List<Enemy>();

            attackDamage = setting.AttackDamage;
            attackRange = setting.AttackRange;
            fireRate = setting.FireRate;

            targetType = setting.TargetType;
            aimType = setting.AimType;

            var _Range = GetComponent<SphereCollider>();
            if (_Range != null)
            {
                _Range.radius = attackRange;
                _Range.isTrigger = false;
            }

            shootTimer = 0;
        }

        private void Start()
        {
            var _Range = GetComponent<SphereCollider>();
            if (_Range != null)
            {
                _Range.radius = attackRange;
                _Range.isTrigger = false;
            }

            shootTimer = 0;
        }

        public Enemy GetEnemyTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
            enemies.Clear();

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    var enemy = collider.GetComponent<Enemy>();
                    if (enemy != null && !enemy.IsDie && enemy.gameObject.activeSelf)
                    {
                        enemies.Add(enemy);
                    }
                }
            }

            if (enemies.Count == 0)
                return null;

            switch (aimType)
            {
                case TowerAimType.Farthest:
                    return GetFarthest();
                case TowerAimType.Farthest_Less_Health:
                    return GetFarthestLessHealth();
                case TowerAimType.Farthest_Most_Health:
                    return GetFarthestMostHealth();
                case TowerAimType.Nearest:
                    return GetNearest();
                case TowerAimType.Nearest_Less_Health:
                    return GetNearestLessHealth();
                case TowerAimType.Nearest_Most_Health:
                    return GetNearestMostHealth();
                default:
                    return null;
            }
        }

        private Enemy GetNearest()
        {
            Enemy nearest = null;
            float minDist = float.MaxValue;
            
            foreach (var enemy in enemies)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = enemy;
                }
            }
            return nearest;
        }

        private Enemy GetFarthest()
        {
            Enemy farthest = null;
            float maxDist = float.MinValue;
            
            foreach (var enemy in enemies)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist > maxDist)
                {
                    maxDist = dist;
                    farthest = enemy;
                }
            }
            return farthest;
        }

        private Enemy GetNearestLessHealth()
        {
            Enemy target = null;
            float minDist = float.MaxValue;
            float minHealth = float.MaxValue;
            
            foreach (var enemy in enemies)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist < minDist || (dist == minDist && enemy.CurrentHealth < minHealth))
                {
                    minDist = dist;
                    minHealth = enemy.CurrentHealth;
                    target = enemy;
                }
            }
            return target;
        }

        private Enemy GetNearestMostHealth()
        {
            Enemy target = null;
            float minDist = float.MaxValue;
            float maxHealth = float.MinValue;
            
            foreach (var enemy in enemies)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist < minDist || (dist == minDist && enemy.CurrentHealth > maxHealth))
                {
                    minDist = dist;
                    maxHealth = enemy.CurrentHealth;
                    target = enemy;
                }
            }
            return target;
        }

        private Enemy GetFarthestLessHealth()
        {
            Enemy target = null;
            float maxDist = float.MinValue;
            float minHealth = float.MaxValue;
            
            foreach (var enemy in enemies)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist > maxDist || (dist == maxDist && enemy.CurrentHealth < minHealth))
                {
                    maxDist = dist;
                    minHealth = enemy.CurrentHealth;
                    target = enemy;
                }
            }
            return target;
        }

        private Enemy GetFarthestMostHealth()
        {
            Enemy target = null;
            float maxDist = float.MinValue;
            float maxHealth = float.MinValue;
            
            foreach (var enemy in enemies)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist > maxDist || (dist == maxDist && enemy.CurrentHealth > maxHealth))
                {
                    maxDist = dist;
                    maxHealth = enemy.CurrentHealth;
                    target = enemy;
                }
            }
            return target;
        }

        private void Update()
        {
            if (IsPause)
                return;

            shootTimer += Time.deltaTime;
            if (shootTimer >= fireRate)
            {
                shootTimer = 0;
                ExecuteProjectile();
            }

            var _enemy = GetEnemyTarget();
            if (_enemy)
                projectileSpot.rotation = Quaternion.LookRotation((_enemy.transform.position - projectileSpot.position).normalized);
        }

        private void ExecuteProjectile()
        {
            projectile?.Shoot<Tower>(this);
        }
    }

}
