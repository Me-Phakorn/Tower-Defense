using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using TowerDefense.Database;
using UnityEngine.EventSystems;
using System.Linq;

namespace TowerDefense.UI
{
    public class TowerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IDropHandler
    {
        public TextMeshProUGUI nameUI;
        public TextMeshProUGUI damageUI;
        public TextMeshProUGUI rangeUI;
        public TextMeshProUGUI firerateUI;
        public TextMeshProUGUI priceUI;

        public Image iconImage;

        private UnitTower tower;
        public UnitTower Tower => tower;

        private Vector3 originPosition;
        private Vector3 currentPosition;

        private bool IsDrag = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!IsDrag)
            {
                originPosition = ((RectTransform)transform).localPosition;

                ((RectTransform)transform).localPosition = new Vector2(originPosition.x, originPosition.y + 50);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ((RectTransform)transform).localPosition = originPosition;
        }

        public void SetupUI(UnitTower tower)
        {
            this.tower = tower;

            nameUI.text = tower.towerName;

            damageUI.text = tower.AttackDamage.ToString();
            rangeUI.text = tower.AttackRange.ToString();
            firerateUI.text = tower.FireRate.ToString();
            // priceUI.text = tower.AttackDamage.ToString();

            iconImage.sprite = tower.UnitIcon;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDrag = true;

            GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            currentPosition = eventData.position;
            ((RectTransform)transform).position = currentPosition;
        }

        public void OnDrop(PointerEventData eventData)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);

            Debug.DrawRay(ray.origin, ray.direction * 20, Color.blue, 3);

            RaycastHit[] hits = Physics.RaycastAll(ray, 20).Where(r => r.collider.CompareTag("Base")).ToArray();
            if (hits.Length == 1)
                hits[0].collider.GetComponent<TowerBase>()?.Construction(tower.UnitPrefab, tower);

            ((RectTransform)transform).localPosition = originPosition;

            GetComponentInParent<HorizontalLayoutGroup>().enabled = true;

            IsDrag = false;
        }
    }
}
