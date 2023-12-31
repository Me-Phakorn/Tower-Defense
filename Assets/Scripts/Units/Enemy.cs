using System.Collections.Generic;
using System.Linq;
using TowerDefense.Database;
using TowerDefense.Display;
using UnityEngine;

namespace TowerDefense
{
    using Pooling;
    public class Enemy : PoolRelease, IDamageable, IEffectStatus
    {
        [Header("Setting")]
        [SerializeField] private EnemyType type;
        [SerializeField] private float attackDamage = 1;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float maximumHealth = 100;

        private float currentHealth = 100;
        private float baseSpeed = 5f;

        private int monsterMoney = 0;

        private Transform[] waypoints;

        private int currentWaypointIndex = 0;

        public bool IsDie => currentHealth <= 0;
        public bool IsPause { get; set; } = false;

        public bool HasPath => waypoints != null && waypoints.Length > 0;

        public float CurrentHealth => currentHealth;

        public int MonsterMoney => monsterMoney;

        private IDamageable target;

        [Header("Effect Status"), SerializeField]
        private List<Effect.Stack> effectStacks = new List<Effect.Stack>();

        public bool HasEffect => effectStacks != null && effectStacks.Count() > 0;

        public EnemyType Type => type;

        private Animator animator;

        private Collider _collider;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            _collider = GetComponent<Collider>();
        }

        public void Initialize(IEnemySetting setting, Transform[] waypoints, IDamageable target)
        {
            this.target = target;

            attackDamage = setting.AttackDamage;

            maximumHealth = setting.BaseHealth;
            currentHealth = maximumHealth;

            baseSpeed = setting.BaseSpeed;
            movementSpeed = baseSpeed;

            monsterMoney = setting.MonsterMoney;

            currentWaypointIndex = 0;

            this.waypoints = waypoints;

            effectStacks = new List<Effect.Stack>();
            _collider.enabled = true;

            animator.SetTrigger("Idle");
            animator.SetBool("Walk", true);

            IsPause = false;

            if (HasPath)
                transform.position = waypoints[0].position;
        }

        private void Update()
        {
            if (HasEffect)
            {
                var _effectStacks = new List<Effect.Stack>(effectStacks);
                foreach (var _effect in _effectStacks)
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

            if (currentWaypointIndex < waypoints.Length)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, movementSpeed * Time.deltaTime);

                if (waypoints[currentWaypointIndex].position - transform.position != Vector3.zero)
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(waypoints[currentWaypointIndex].position - transform.position), Time.deltaTime * 7);
            }
        }

        private void DestroyTower()
        {
            IsPause = true;

            target?.Damage(attackDamage);

            Release();
        }

        public void Damage(float amount)
        {
            DisplayDamage.Instance?.Display(transform, amount);

            currentHealth -= amount;

            if (currentHealth <= 0 && !IsPause)
            {
                IsPause = false;

                GameController.Instance?.AddMoney(monsterMoney);

                _collider.enabled = false;
                animator.SetTrigger("Die");
                Invoke("Release", 2);
            }
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
