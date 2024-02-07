using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Game.UI
{
    public class LevelChangeScreen : BaseScreen
    {
        [SerializeField] private TMP_Text _levelText;
        public override void Hide()
        {
            base.Hide();
            _levelText.text = "";
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            if (data != null)
            {
                int level = (int)data;
                _levelText.text = "Day " + level.ToString();
            }
        }
    }

}