using System.Collections.Generic;
using TowerDefense.Database;
using UnityEngine;

namespace TowerDefense.Setting
{
    public abstract class LevelCondition : ScriptableObject
    {
        [Header("Level Setting")]
        [SerializeField]
        protected EnemyCollection enemyCollection;

        [SerializeField]
        protected int moneyPerWave = 200;

        [SerializeField]
        protected float enemyFrequency = 0.5f;

        [SerializeField]
        protected float wavePerTime = 60;

        [SerializeField]
        protected int enemiesPreWave = 10;

        public float EnemyFrequency => enemyFrequency;
        public float WavePerTime => wavePerTime;
        public int EnemiesPreWave => enemiesPreWave;
        public int MoneyPerWave => moneyPerWave;

        public Enemy GetEnemy(int index)
        {
            if (index >= enemyCollection.Collections.Length)
                return null;

            return enemyCollection.Collections[index].Enemy;
        }

        public Enemy GetRandomEnemy()
        {
            return GetEnemy(Random.Range(0, enemyCollection.Collections.Length));
        }


        public abstract IEnemySetting GetEnemySetting(ILevelSetting levelSetting, Enemy enemy);
    }
}