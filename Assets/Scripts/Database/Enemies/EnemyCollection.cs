using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Database
{
    [CreateAssetMenu(fileName = "Enemy Collection", menuName = "Tower-Defense/Enemy/Collection", order = 0)]
    public class EnemyCollection : ScriptableObject
    {
        [System.Serializable]
        public struct Collection : IEnemySetting
        {
            [SerializeField]
            private Enemy enemy;

            [SerializeField]
            private EnemyType enemyType;

            [SerializeField]
            private int baseDamage;
            [SerializeField]
            private float baseHealth;
            [SerializeField]
            private float baseSpeed;

            public Enemy Enemy => enemy;

            public EnemyType Type { get => enemyType; }

            public int AttackDamage => baseDamage;

            public float BaseHealth { get => baseHealth; set { baseHealth = value; } }
            public float BaseSpeed { get => baseSpeed; set { baseSpeed = value; } }
        }

        [SerializeField]
        private List<Collection> enemies = new List<Collection>();

        public Collection[] Collections => enemies.ToArray();
    }


}