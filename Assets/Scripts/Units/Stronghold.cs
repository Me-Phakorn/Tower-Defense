using UnityEngine;

namespace TowerDefense
{
    public class Stronghold : MonoBehaviour , IDamageable
    {
        [Header("Setting")]
        [SerializeField] private float MaximumHealth = 100;

        private float currentHealth = 100;

        public float Damages => 0;
        public float Health => currentHealth;
        public float MaxHealth => MaximumHealth;

        public bool IsDestroy => currentHealth <= 0;

        public void Damage(float amount)
        {
            currentHealth -= amount;
        }
    }
}
