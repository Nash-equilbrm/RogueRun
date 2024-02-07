using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;



namespace Game.GameFlow
{
    public class GameMainMenuState : State<GameManager>
    {
        public GameMainMenuState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.HideAllScreens();
            UIManager.Instance.HideAllOverlaps();
            UIManager.Instance.ShowScreen<MainMenuScreen>(forceShowData: true);
            _context.Register(EventID.StartGame, StartGame);

            AudioManager.Instance.musicSource.Play();
        }

       
        public override void Exit()
        {
            base.Exit();
            _context.Unregister(EventID.StartGame, StartGame);
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        private void StartGame(object obj)
        {
            _context.ChangeState(GameState.NewLevel);
        }

    }
}

