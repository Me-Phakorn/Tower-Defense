using System.Collections;
using System.Collections.Generic;
using TowerDefense.Pooling;
using UnityEngine;

namespace TowerDefense
{
    public class TowerBase : MonoBehaviour
    {
        [SerializeField]
        private Tower tower;

        public bool Construction(Tower towerPrefab, int towerPrice, ITowerSetting setting)
        {
            if (tower != null)
                return false;

            if (!GameController.Instance.PayTower(towerPrice))
                return false;

            tower = Instantiate(towerPrefab, transform);

            tower.transform.position = transform.position;
            tower.transform.rotation = transform.rotation;

            tower.Construction(setting);

            return false;
        }

        public void DeConstruction()
        {
            if (!tower)
                return;
        }
    }
}
