using Game.UI;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace Game.GameFlow
{
    public class GameOverState : State<GameManager>
    {
        public GameOverState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.ShowScreen<GameOverScreen>(data: _context.CurLevel, forceShowData: true);
            _context.Register(EventID.BackToMenu, OnBackToMenu);
        }

        public override void Exit()
        {
            base.Exit();
            _context.ResetGame();
            _context.Unregister(EventID.BackToMenu, OnBackToMenu);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        private void OnBackToMenu(object data = null)
        {
            _context.ChangeState(GameState.MainMenu);
        }
    }
}

