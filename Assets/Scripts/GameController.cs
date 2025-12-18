using System.Collections;
using System.Collections.Generic;
using TowerDefense.Database;
using TowerDefense.Pooling;
using TowerDefense.Setting;
using TowerDefense.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class GameController : MonoBehaviour, ILevelSetting
    {
        private static GameController instance;
        public static GameController Instance => instance;

        [Header("Level Setting")]
        [SerializeField] private List<UnitTower> towerCollection;
        [SerializeField] private LevelCondition levelCondition;

        [Header("Component")]
        [SerializeField] private Stronghold stronghold;
        [SerializeField] private UIManager UIManager;

        [Header("Setting")]
        [SerializeField] private Transform[] wayPoints;

        [Header("Game Setting")]
        [SerializeField] private int strongholdHealth = 100;
        [SerializeField, Range(1, 3)] private float gameSpeed = 1;
        [SerializeField] private float enemyFrequency = 0;
        [SerializeField] private int startMoney = 300;
        [SerializeField] private int moneyPerWave = 0;
        [SerializeField] private float wavePerTime = 0;
        [SerializeField] private float enemiesPreWave = 0;

        [Header("Runtime")]
        [SerializeField] private int totalMoney = 1;
        [SerializeField] private float totalTimer = 0;
        [SerializeField] private int totalWave = 1;

        [Space, SerializeField]
        private UnityEvent<int> onNextWave;
        [Space]
        public UnityEvent<int> onMoneyUpdate;

        public float GameSpeed => gameSpeed;

        private bool isStart = false;
        public bool IsStart => isStart;

        public bool IsLose => stronghold.IsDestroy;

        private bool isPause = false;
        public bool IsPause => isPause;

        public float GameTime { get => totalTimer; set { totalTimer = value; } }
        public int GameWave { get => totalWave; set { totalWave = value; } }

        public int Money => totalMoney;
        public int Health => stronghold.Health;

        private float timer = 0;

        private List<Enemy> enemies = new List<Enemy>();

        private IEnumerator enemiesAttack;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            ObjectPool.ClearAll();

            totalTimer = 0;
            totalWave = 1;
            timer = 0;

            AddMoney(startMoney);

            UIManager.SetupTowerUI(towerCollection.ToArray());

            stronghold.Setup(strongholdHealth);
        }

        public void OnGameStart()
        {
            wavePerTime = levelCondition.WavePerTime;
            enemiesPreWave = levelCondition.EnemiesPreWave;
            enemyFrequency = levelCondition.EnemyFrequency;
            moneyPerWave = levelCondition.MoneyPerWave;

            ChangeSpeed(1);

            StartCoroutine(enemiesAttack = EnemiesAttack(5));

            isStart = true;
        }

        private void Update()
        {
            if (IsPause || !IsStart)
                return;

            timer += Time.deltaTime * gameSpeed;
            totalTimer += Time.deltaTime * gameSpeed;

            if (timer >= wavePerTime)
                NextWave();
        }

        private void NextWave()
        {
            timer = 0;
            totalWave++;

            onNextWave?.Invoke(totalWave);
            StartCoroutine(enemiesAttack = EnemiesAttack());

            AddMoney(moneyPerWave);
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

        public void AddMoney(int amount)
        {
            totalMoney += amount;

            onMoneyUpdate.Invoke(totalMoney);
        }

        public bool PayTower(int price)
        {
            if (price > totalMoney)
                return false;

            totalMoney -= price;
            onMoneyUpdate.Invoke(totalMoney);
            return true;
        }

        public void GameOver()
        {
            if (isPause)
                return;

            isPause = true;
            isStart = false;

            UIManager.GameOver();
        }

        public void GameRestart()
        {
            SceneManager.LoadScene("Tower Battle");
        }

        private IEnumerator EnemiesAttack(int delay = 0)
        {
            var _enemy = levelCondition.GetRandomEnemy();

            yield return new WaitForSeconds(delay);

            for (int i = 0; i < enemiesPreWave; i++)
            {
                var _obj = ObjectPool.GetPool(_enemy.gameObject, wayPoints[0].position, Quaternion.LookRotation(wayPoints[1].position - wayPoints[0].position));
                _obj.GetComponent<Enemy>().Initialize(levelCondition.GetEnemySetting(this, _enemy), wayPoints, stronghold);

                yield return new WaitForSeconds(enemyFrequency);
            }
        }
    }
}
