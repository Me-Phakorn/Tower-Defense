using UnityEngine;

namespace TowerDefense
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Header("Setting")]
        [SerializeField] private EnemyType type;
        [SerializeField] private float damage = 1;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float MaximumHealth = 100;

        private float currentHealth = 100;

        private Transform[] waypoints;

        private int currentWaypointIndex = 0;

        public float Damages => damage;
        public float Health => currentHealth;
        public float MaxHealth => MaximumHealth;

        public bool IsDie => currentHealth <= 0;
        public bool IsPause { get; set; } = false;

        public bool HasPath => waypoints != null || waypoints.Length > 0;

        private IDamageable target;

        public void Initialize(Transform[] waypoints, IDamageable target)
        {
            currentHealth = MaximumHealth;

            currentWaypointIndex = 0;

            this.waypoints = waypoints;

            if (HasPath)
                transform.position = waypoints[0].position;
        }

        private void Update()
        {
            if (IsPause || !HasPath || IsDie)
                return;

            if (transform.position == waypoints[currentWaypointIndex].position)
            {
                currentWaypointIndex++;

                if (currentWaypointIndex >= waypoints.Length)
                    DestroyTower();
            }

            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, movementSpeed * Time.deltaTime);
        }

        private void DestroyTower()
        {
            IsPause = true;
            gameObject.SetActive(false);

            target?.Damage(damage);
        }

        public void Damage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
                IsPause = false;
        }
    }
}
