using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace TowerDefense.UI
{
    public class TurnUI : MonoBehaviour
    {
        public TextMeshProUGUI turnUI;

        public void UpdateTurn(int turnNumber)
        {
            turnUI.text = turnNumber.ToString("0");
        }
    }

}