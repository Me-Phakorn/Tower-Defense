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

        public void SetupTowerUI(UnitTower[] unitTowers)
        {
            foreach (var item in unitTowers)
                Instantiate(towerUIPrefab, towerUITarget).SetupUI(item);

            Invoke("DisableLayoutGroup", 0.1f);
        }

        private void DisableLayoutGroup()
        {
            towerUITarget.GetComponent<HorizontalLayoutGroup>().enabled = false;
        }
    }
}
