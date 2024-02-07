using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;


namespace Game.GameFlow
{
    public class GamePlayState : State<GameManager>
    {
        private bool enemiesMoving;
        private float turnDelay = 0.1f;

        public GamePlayState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _context.Register(EventID.GameOver, OnGameOver);
            _context.Register(EventID.NewLevel, OnNewLevelLoad);
            UIManager.Instance.ShowOverlap<GameplayOverlap>(forceShowData: true);
        }

        private void OnGameOver(object obj)
        {
            _context.ChangeState(GameState.Gameover);
        }

        public override void Exit()
        {
            base.Exit();
            _context.Unregister(EventID.GameOver, OnGameOver);
            _context.Unregister(EventID.NewLevel, OnNewLevelLoad);

        }

        private void OnNewLevelLoad(object obj)
        {
            Debug.Log("game play state OnNewLevelLoad");
            _context.ChangeState(GameState.NewLevel);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (_context.PlayersTurn || enemiesMoving || _context.DoingSetup)
                return;

            _context.StartCoroutine(MoveEnemies());
        }

        IEnumerator MoveEnemies()
        {
            enemiesMoving = true;

            yield return new WaitForSeconds(turnDelay);

            if (_context.Enemies.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }

            for (int i = 0; i < _context.Enemies.Count; i++)
            {
                _context.Enemies[i].MoveEnemy();

                yield return new WaitForSeconds(_context.Enemies[i].moveTime);
            }
            _context.PlayersTurn = true;

            enemiesMoving = false;
        }
    }

}
