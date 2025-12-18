using System.Collections;
using System.Collections.Generic;
using TowerDefense.Database;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Prefabs")]
        [SerializeField]
        private TowerUI towerUIPrefab;

        [Header("UI Target")]
        [SerializeField]
        private RectTransform towerUITarget;

        [SerializeField]
        private GameObject gameOverPanel;

        private List<TowerUI> towerUIs = new List<TowerUI>();

        public void SetupTowerUI(UnitTower[] unitTowers)
        {
            foreach (var item in unitTowers)
            {
                var towerUI = Instantiate(towerUIPrefab, towerUITarget);
                towerUI.SetupUI(item);
                towerUIs.Add(towerUI);
            }

            Invoke("DisableLayoutGroup", 0.1f);
            
            UpdateTowerUIStates(GameController.Instance.Money);
        }

        private void Start()
        {
            var gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                gameController.onMoneyUpdate.AddListener(UpdateTowerUIStates);
            }
        }

        private void OnDestroy()
        {
            var gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                gameController.onMoneyUpdate.RemoveListener(UpdateTowerUIStates);
            }
        }

        private void UpdateTowerUIStates(int currentMoney)
        {
            foreach (var towerUI in towerUIs)
            {
                towerUI.UpdateMoney(currentMoney);
            }
        }

        public void GameOver()
        {
            gameOverPanel.SetActive(true);
        }

        private void DisableLayoutGroup()
        {
            towerUITarget.GetComponent<HorizontalLayoutGroup>().enabled = false;
        }
    }
}
