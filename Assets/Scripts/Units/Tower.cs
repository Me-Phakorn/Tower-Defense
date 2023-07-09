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

        public EnemyType TargetType => targetType;
        public TowerAimType AimType => aimType;

        public List<Enemy> enemies = new List<Enemy>();

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

        private void Start()
        {
            var _Range = GetComponent<SphereCollider>();
            _Range.radius = attackRange;

            shootTimer = 0;
        }

        public Enemy GetEnemyTarget()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRange, transform.up);
            enemies = hits.Where(h => h.collider.CompareTag("Enemy")).Select(h => h.collider.GetComponent<Enemy>()).ToList();

            var _enemies = enemies.Where(e => !e.IsDie && e.gameObject.activeSelf);

            switch (aimType)
            {
                case TowerAimType.Farthest:
                    {
                        return _enemies.OrderByDescending(e => Vector3.Distance(e.transform.position, transform.position)).FirstOrDefault();
                    }
                case TowerAimType.Farthest_Less_Health:
                    {
                        return _enemies.OrderByDescending(e => Vector3.Distance(e.transform.position, transform.position))
                         .ThenBy(e => e.CurrentHealth).FirstOrDefault();
                    }
                case TowerAimType.Farthest_Most_Health:
                    {
                        return _enemies.OrderByDescending(e => Vector3.Distance(e.transform.position, transform.position))
                        .ThenByDescending(e => e.CurrentHealth).FirstOrDefault();
                    }
                case TowerAimType.Nearest:
                    {
                        return _enemies.OrderBy(e => Vector3.Distance(e.transform.position, transform.position)).FirstOrDefault();
                    }
                case TowerAimType.Nearest_Less_Health:
                    {
                        return _enemies.OrderBy(e => Vector3.Distance(e.transform.position, transform.position))
                         .ThenBy(e => e.CurrentHealth).FirstOrDefault();
                    }
                case TowerAimType.Nearest_Most_Health:
                    {
                        return _enemies.OrderBy(e => Vector3.Distance(e.transform.position, transform.position))
                        .ThenByDescending(e => e.CurrentHealth).FirstOrDefault();
                    }
                default:
                    return null;
            }
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
