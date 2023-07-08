using System.Collections.Generic;
using System.Linq;
using TowerDefense.Database;
using TowerDefense.Display;
using UnityEngine;

namespace TowerDefense
{
    public class Enemy : MonoBehaviour, IDamageable, IEffectStatus, IEnemySetting
    {
        [Header("Setting")]
        [SerializeField] private EnemyType type;
        [SerializeField] private float attackDamage = 1;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float maximumHealth = 100;

        private float currentHealth = 100;

        private Transform[] waypoints;

        private int currentWaypointIndex = 0;

        public bool IsDie => currentHealth <= 0;
        public bool IsPause { get; set; } = false;

        public bool HasPath => waypoints != null && waypoints.Length > 0;

        public int AttackDamage => (int)attackDamage;

        private float baseSpeed = 5f;
        public float BaseSpeed => baseSpeed;
        public float BaseHealth => maximumHealth;

        private IDamageable target;

        [Header("Effect Status"), SerializeField]
        private List<Effect.Stack> effectStacks = new List<Effect.Stack>();

        public bool HasEffect => effectStacks != null && effectStacks.Count() > 0;

        public void Initialize(IEnemySetting setting, Transform[] waypoints, IDamageable target)
        {
            maximumHealth = setting.BaseHealth;
            currentHealth = maximumHealth;

            baseSpeed = setting.BaseSpeed;
            movementSpeed = baseSpeed;

            currentWaypointIndex = 0;

            this.waypoints = waypoints;

            if (HasPath)
                transform.position = waypoints[0].position;
        }

        private void Update()
        {
            if (HasEffect)
            {
                foreach (var _effect in effectStacks)
                {
                    _effect.effectTimer += Time.deltaTime;

                    if (_effect.effectTimer >= _effect.effect.EffectDuration)
                        RemoveEffectStatus(_effect.effect);
                }
            }

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

            target?.Damage(AttackDamage);
        }

        public void Damage(float amount)
        {
            DisplayDamage.Instance?.Display(transform, amount);

            currentHealth -= amount;

            if (currentHealth <= 0)
                IsPause = false;
        }

        public void AddEffectStatus<T>(float baseDamage, T effect) where T : Effect
        {
            var _stacks = effectStacks.Where(e => e.effect == effect).FirstOrDefault();

            if (_stacks == null)
                effectStacks.Add(new Effect.Stack(baseDamage, effect));
            else
                _stacks.effectTimer = 0;

            if (effect.GetType() == typeof(SlowEffect))
            {
                var _effect = effect as SlowEffect;
                movementSpeed = baseSpeed - (baseSpeed * (_effect.SlowPercent / 100));
            }
            else if (effect.GetType() == typeof(BurnEffect))
            {

            }
        }

        private void RemoveEffectStatus<T>(T effect) where T : Effect
        {
            effectStacks.Remove(effectStacks.Where(e => e.effect == effect).FirstOrDefault());

            if (effect.GetType() == typeof(SlowEffect))
            {
                var _effect = effect as SlowEffect;
                movementSpeed = baseSpeed;
            }
            else if (effect.GetType() == typeof(BurnEffect))
            {

            }
        }
    }
}
