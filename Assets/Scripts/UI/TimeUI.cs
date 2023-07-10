using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace TowerDefense.UI
{
    public class TimeUI : MonoBehaviour
    {
        public TextMeshProUGUI timeUI;

        GameController gameController;

        private void Start()
        {
            gameController = GameController.Instance;
        }

        private void Update()
        {
            if (!gameController)
                return;

            TimeSpan time = TimeSpan.FromSeconds(gameController.GameTime);
            timeUI.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
        }
    }

}