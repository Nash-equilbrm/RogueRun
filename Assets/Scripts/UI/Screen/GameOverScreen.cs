using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    public class GameOverScreen : BaseScreen
    {
        [SerializeField] private TMP_Text _gameOverText;
        [SerializeField] private Button _exitBtn;
        public override void Hide()
        {
            base.Hide();
            _gameOverText.text = "";
            _exitBtn.onClick.RemoveListener(BackToMainMenu);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            if(data != null)
            {
                int endLevel = (int)data;
                _gameOverText.text = "After " + endLevel.ToString() + " days you die"; 
            }

            _exitBtn.onClick.AddListener(BackToMainMenu);
        }

        private void BackToMainMenu()
        {
            this.Broadcast(EventID.BackToMenu);
        }
    }
}

