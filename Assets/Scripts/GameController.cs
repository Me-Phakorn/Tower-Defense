using System.Collections;
using System.Collections.Generic;
using TowerDefense.Pooling;
using TowerDefense.Setting;
using UnityEngine;

namespace TowerDefense
{
    public delegate void OnEnemyCallback(Enemy enemy);

    public class GameController : MonoBehaviour, ILevelSetting
    {
        [Header("Level Setting")]
        [SerializeField] private LevelCondition levelCondition;

        [Header("Component")]
        [SerializeField] private Stronghold stronghold;

        [Header("Setting")]
        [SerializeField] private Transform[] wayPoints;

        [Header("Game Setting")]
        [SerializeField, Range(1, 3)] private float gameSpeed = 1;
        [SerializeField] private float enemyFrequency = 0;
        [SerializeField] private float wavePerTime = 0;
        [SerializeField] private float enemiesPreWave = 0;

        [Header("Runtime")]
        [SerializeField] private float totalTimer = 0;
        [SerializeField] private float totalWave = 1;

        public float GameSpeed => gameSpeed;

        public bool IsLose => stronghold.IsDestroy;

        private bool isPause;
        public bool IsPause => isPause;

        public float GameTime { get => totalTimer; set { totalTimer = value; } }
        public float GameWave { get => totalWave; set { totalWave = value; } }

        private List<Enemy> enemies = new List<Enemy>();

        private void Start()
        {
            OnGameStart();
        }

        public void OnGameStart()
        {
            wavePerTime = levelCondition.WavePerTime;
            enemiesPreWave = levelCondition.EnemiesPreWave;
            enemyFrequency = levelCondition.EnemyFrequency;

            ChangeSpeed(1);

            totalTimer = 0;
            totalWave = 1;

            StartCoroutine(EnemiesAttack());
        }

        public void GamePause(bool isPause)
        {
            this.isPause = isPause;

            if (isPause)
                Time.timeScale = 0;
            else
                Time.timeScale = gameSpeed;
        }

        public void ChangeSpeed(float gameSpeed)
        {
            if (gameSpeed < 1 || gameSpeed > 3)
                return;

            this.gameSpeed = gameSpeed;
            Time.timeScale = gameSpeed;
        }

        private IEnumerator EnemiesAttack()
        {
            var _enemy = levelCondition.GetRandomEnemy();

            for (int i = 0; i < enemiesPreWave; i++)
            {
                var _obj = ObjectPool.GetPool(_enemy.gameObject, wayPoints[0].position, Quaternion.identity);
                _obj.GetComponent<Enemy>().Initialize(levelCondition.GetEnemySetting(this, _enemy), wayPoints, stronghold);

                yield return new WaitForSeconds(enemyFrequency);
            }

            yield return new WaitForSeconds(20);

            for (int i = 0; i < enemiesPreWave; i++)
            {
                var _obj = ObjectPool.GetPool(_enemy.gameObject, wayPoints[0].position, Quaternion.identity);
                _obj.GetComponent<Enemy>().Initialize(levelCondition.GetEnemySetting(this, _enemy), wayPoints, stronghold);

                yield return new WaitForSeconds(enemyFrequency);
            }
        }

        private void OnEnemyDie(Enemy enemy)
        {

        }
    }
}
