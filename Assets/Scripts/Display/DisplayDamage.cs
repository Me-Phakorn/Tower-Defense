using DamageNumbersPro;
using UnityEngine;

namespace TowerDefense.Display
{
    public class DisplayDamage : MonoBehaviour
    {
        private static DisplayDamage instance;
        public static DisplayDamage Instance => instance;

        public DamageNumber numberPrefab;

        public Vector3 offset;

        private void Awake()
        {
            instance = this;
        }

        public void Display(Transform target, float damageNumber)
        {
            numberPrefab.Spawn(target.position + offset, (int)damageNumber);
        }
    }
}