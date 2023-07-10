using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense
{
    public class Stronghold : MonoBehaviour, IDamageable
    {
        [Header("Setting")]
        [SerializeField] private int maximumHealth = 100;

        [SerializeField]
        private int currentHealth = 100;

        public int Health => currentHealth;
        public int MaxHealth => maximumHealth;

        public bool IsDestroy => currentHealth <= 0;

        [Space, SerializeField]
        private UnityEvent<int> onHealthUpdate;

        public void Setup(int maximumHealth)
        {
            this.maximumHealth = maximumHealth;
            currentHealth = maximumHealth;

            onHealthUpdate?.Invoke(currentHealth);
        }

        public void Damage(float amount)
        {
            currentHealth -= Mathf.CeilToInt(amount);

            if(currentHealth < 0)
                currentHealth = 0;

            if (IsDestroy)
                GameController.Instance?.GameOver();

            onHealthUpdate?.Invoke(currentHealth);
        }
    }
}
