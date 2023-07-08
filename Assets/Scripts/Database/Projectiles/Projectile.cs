using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Database
{
    public abstract class Projectile : ScriptableObject
    {
        [Header("General")]
        public string projectileName;
        [TextArea] public string description;

        [Header("Prefabs")]
        [SerializeField]
        protected GameObject muzzlePrefab;
        [SerializeField]
        protected GameObject projectilePrefab;

        [Header("Projectile Setting")]
        [SerializeField, Range(0.5f, 3f)]
        protected float multipleDamage;

        [SerializeField, Range(1f, 20f)]
        protected float projectileSpeed;

        [Header("Effect Setting")]
        [SerializeField]
        protected Effect effect;

        protected float FinalDamage(float baseDamage)
        {
            return baseDamage * multipleDamage;
        }

        public abstract void Shoot<T>(T source) where T : Tower;
        
        public abstract void Dispose();

    }
}
