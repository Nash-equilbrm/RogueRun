using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using UnityEngine;


namespace Game.UI
{
    public class GameplayOverlap : BaseOverlap
    {
        [SerializeField] private TMP_Text _foodText;
        public override void Hide()
        {
            base.Hide();
            this.Unregister(EventID.OnFoodChange, ChangeFoodText);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            this.Register(EventID.OnFoodChange, ChangeFoodText);
            _foodText.text = "Food: " + GameManager.Instance.playerCurrentFoodPoints.ToString();
        }

        private void ChangeFoodText(object data)
        {
            if(data != null)
            {
                string text = (string)data;
                _foodText.text = text;
            }
        }
    }
}

