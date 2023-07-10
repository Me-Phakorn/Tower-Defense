using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Database
{
    [CreateAssetMenu(fileName = "Unit Tower", menuName = "Tower-Defense/Tower", order = 0)]
    public class UnitTower : ScriptableObject, ITowerSetting
    {
        [Header("General")]
        public string towerName;
        [TextArea] public string description;

        [SerializeField]
        public Tower unitPrefab;

        [SerializeField]
        private Sprite unitIcon;

        [Header("Tower Setting")]
        [SerializeField]
        private Projectile projectile;

        [SerializeField]
        private EnemyType targetType;
        [SerializeField]
        private TowerAimType aimType;

        [SerializeField]
        private int towerPrice = 0;

        [SerializeField, Range(1, 10)]
        private int attackDamage = 1;

        [SerializeField, Range(0.1f, 5f)]
        private float fireRate = 1;
        [SerializeField, Range(3, 7)]
        private int attackRange = 3;

        public Sprite UnitIcon => unitIcon;

        public Tower UnitPrefab => unitPrefab;

        public Projectile Projectile => projectile;

        public EnemyType TargetType => targetType;

        public TowerAimType AimType => aimType;

        public float FireRate => fireRate;
        public int AttackRange => attackRange;
        public int AttackDamage => attackDamage;

        public int TowerPrice => towerPrice;
    }
}
