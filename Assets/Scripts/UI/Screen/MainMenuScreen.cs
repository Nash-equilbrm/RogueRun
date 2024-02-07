using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;


namespace Game.UI
{
    public class MainMenuScreen : BaseScreen
    {
        [SerializeField] private Button _startGameBtn;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;

        public override void Hide()
        {
            base.Hide();
            _startGameBtn.onClick.RemoveListener(StartGameBtnOnClick);
            _sfxSlider.onValueChanged.RemoveListener(SFXSliderOnValueChanged);
            _musicSlider.onValueChanged.RemoveListener(MusicSliderOnValueChanged);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            _startGameBtn.onClick.AddListener(StartGameBtnOnClick);
            _sfxSlider.onValueChanged.AddListener(SFXSliderOnValueChanged);
            _musicSlider.onValueChanged.AddListener(MusicSliderOnValueChanged);

            _sfxSlider.value = AudioManager.Instance.sfxSource.volume;
            _musicSlider.value = AudioManager.Instance.musicSource.volume;

        }

        private void MusicSliderOnValueChanged(float value)
        {
            this.Broadcast(EventID.OnMusicChanged, value);
        }

        private void SFXSliderOnValueChanged(float value)
        {
            this.Broadcast(EventID.OnSFXChanged, value);
        }

        private void StartGameBtnOnClick()
        {
            this.Broadcast(EventID.StartGame);
        }

    }
}

