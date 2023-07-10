using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace TowerDefense.UI
{
    public class ValueUI : MonoBehaviour
    {
        public TextMeshProUGUI valueUI;

        public void UpdateUI(int value)
        {
            valueUI.text = value.ToString("0");
        }
    }

}